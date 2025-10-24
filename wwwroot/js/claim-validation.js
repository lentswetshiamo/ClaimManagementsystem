// Claim Form Validation and Dynamic Functionality

document.addEventListener('DOMContentLoaded', function () {
    initializeClaimForm();
});

function initializeClaimForm() {
    const claimForm = document.getElementById('claimForm');
    if (!claimForm) return;

    // Initialize claim items counter
    let itemCounter = 1;

    // Add event listeners for dynamic calculations
    const totalHoursInput = document.querySelector('input[name="TotalHours"]');
    const hourlyRateInput = document.querySelector('input[name="HourlyRate"]');

    if (totalHoursInput) {
        totalHoursInput.addEventListener('input', updateClaimSummary);
    }
    if (hourlyRateInput) {
        hourlyRateInput.addEventListener('input', updateClaimSummary);
    }

    // Initialize claim items
    initializeClaimItems();

    // Add item button
    const addItemBtn = document.getElementById('addItem');
    if (addItemBtn) {
        addItemBtn.addEventListener('click', addClaimItem);
    }

    // Document upload handling
    const documentUpload = document.getElementById('documentUpload');
    if (documentUpload) {
        documentUpload.addEventListener('change', handleDocumentUpload);
    }

    // Save draft functionality
    const saveDraftBtn = document.getElementById('saveDraft');
    if (saveDraftBtn) {
        saveDraftBtn.addEventListener('click', saveAsDraft);
    }

    // Form submission validation
    claimForm.addEventListener('submit', validateClaimSubmission);
}

function initializeClaimItems() {
    // Add event listeners to existing claim items
    document.querySelectorAll('.claim-item input').forEach(input => {
        input.addEventListener('input', updateItemCalculations);
    });
}

function addClaimItem() {
    const itemsContainer = document.getElementById('claimItems');
    const newItem = document.createElement('div');
    newItem.className = 'claim-item card mb-2';
    newItem.innerHTML = `
        <div class="card-body">
            <div class="row">
                <div class="col-md-5">
                    <input type="text" class="form-control" placeholder="Module/Description" name="ClaimItems[${itemCounter}].Module" required>
                </div>
                <div class="col-md-3">
                    <input type="number" class="form-control hours-input" placeholder="Hours" name="ClaimItems[${itemCounter}].HoursWorked" step="0.5" min="0.5" required>
                </div>
                <div class="col-md-3">
                    <input type="number" class="form-control rate-input" placeholder="Rate" name="ClaimItems[${itemCounter}].Rate" step="0.01" required>
                </div>
                <div class="col-md-1">
                    <button type="button" class="btn btn-sm btn-outline-danger remove-item">
                        <i class="fas fa-times"></i>
                    </button>
                </div>
            </div>
        </div>
    `;

    itemsContainer.appendChild(newItem);

    // Add event listeners to new inputs
    const inputs = newItem.querySelectorAll('input');
    inputs.forEach(input => {
        input.addEventListener('input', updateItemCalculations);
    });

    // Add remove functionality
    const removeBtn = newItem.querySelector('.remove-item');
    removeBtn.addEventListener('click', function () {
        newItem.remove();
        updateClaimSummary();
    });

    // Enable remove button for all items if there's more than one
    updateRemoveButtons();

    itemCounter++;
}

function updateRemoveButtons() {
    const items = document.querySelectorAll('.claim-item');
    const removeButtons = document.querySelectorAll('.remove-item');

    if (items.length > 1) {
        removeButtons.forEach(btn => btn.disabled = false);
    } else {
        removeButtons.forEach(btn => btn.disabled = true);
    }
}

function updateItemCalculations(event) {
    const item = event.target.closest('.claim-item');
    if (!item) return;

    const hoursInput = item.querySelector('.hours-input');
    const rateInput = item.querySelector('.rate-input');

    if (hoursInput && rateInput) {
        const hours = parseFloat(hoursInput.value) || 0;
        const rate = parseFloat(rateInput.value) || 0;

        // You could display the amount per item if needed
        // const amount = hours * rate;
    }

    updateClaimSummary();
}

function updateClaimSummary() {
    const totalHoursInput = document.querySelector('input[name="TotalHours"]');
    const hourlyRateInput = document.querySelector('input[name="HourlyRate"]');

    const totalHours = parseFloat(totalHoursInput?.value) || 0;
    const hourlyRate = parseFloat(hourlyRateInput?.value) || 0;
    const totalAmount = totalHours * hourlyRate;

    // Update summary display
    const summaryHours = document.getElementById('summaryHours');
    const summaryRate = document.getElementById('summaryRate');
    const summaryTotal = document.getElementById('summaryTotal');

    if (summaryHours) summaryHours.textContent = totalHours.toFixed(1);
    if (summaryRate) summaryRate.textContent = 'R ' + hourlyRate.toFixed(2);
    if (summaryTotal) summaryTotal.textContent = 'R ' + totalAmount.toFixed(2);

    // Validate against business rules
    validateBusinessRules(totalHours, hourlyRate, totalAmount);
}

function validateBusinessRules(totalHours, hourlyRate, totalAmount) {
    const errors = [];

    // Maximum hours validation
    if (totalHours > 160) {
        errors.push('Maximum 160 hours per month allowed');
    }

    // Hourly rate validation
    if (hourlyRate > 500) {
        errors.push('Hourly rate exceeds maximum allowed rate');
    }

    // Minimum hours validation
    if (totalHours < 1) {
        errors.push('Minimum 1 hour required');
    }

    // Display errors
    displayValidationErrors(errors);
}

function displayValidationErrors(errors) {
    let errorContainer = document.getElementById('validationErrors');

    if (!errorContainer) {
        errorContainer = document.createElement('div');
        errorContainer.id = 'validationErrors';
        errorContainer.className = 'alert alert-warning mt-3';

        const form = document.getElementById('claimForm');
        form.parentNode.insertBefore(errorContainer, form);
    }

    if (errors.length > 0) {
        errorContainer.innerHTML = `
            <h6><i class="fas fa-exclamation-triangle me-2"></i>Validation Warnings</h6>
            <ul class="mb-0">
                ${errors.map(error => `<li>${error}</li>`).join('')}
            </ul>
        `;
        errorContainer.style.display = 'block';
    } else {
        errorContainer.style.display = 'none';
    }
}

function handleDocumentUpload(event) {
    const files = event.target.files;
    const uploadedFilesContainer = document.getElementById('uploadedFiles');

    if (!uploadedFilesContainer) return;

    for (let file of files) {
        // Validate file
        const validationResult = validateDocumentFile(file);

        if (validationResult.isValid) {
            addFileToUploadList(file, uploadedFilesContainer);
        } else {
            alert(`File "${file.name}" rejected: ${validationResult.errors.join(', ')}`);
        }
    }
}

function validateDocumentFile(file) {
    const result = {
        isValid: true,
        errors: []
    };

    // Check file size (10MB max)
    const maxSize = 10 * 1024 * 1024;
    if (file.size > maxSize) {
        result.isValid = false;
        result.errors.push('File size exceeds 10MB limit');
    }

    // Check file type
    const allowedTypes = ['.pdf', '.doc', '.docx', '.jpg', '.jpeg', '.png'];
    const fileExtension = '.' + file.name.split('.').pop().toLowerCase();
    if (!allowedTypes.includes(fileExtension)) {
        result.isValid = false;
        result.errors.push('File type not allowed');
    }

    return result;
}

function addFileToUploadList(file, container) {
    const fileElement = document.createElement('div');
    fileElement.className = 'd-flex justify-content-between align-items-center p-2 border rounded mb-2';
    fileElement.innerHTML = `
        <div>
            <i class="fas fa-file me-2 text-emerald"></i>
            <span class="small">${file.name}</span>
            <small class="text-muted ms-2">(${(file.size / 1024 / 1024).toFixed(2)} MB)</small>
        </div>
        <button type="button" class="btn btn-sm btn-outline-danger remove-file">
            <i class="fas fa-times"></i>
        </button>
    `;

    const removeBtn = fileElement.querySelector('.remove-file');
    removeBtn.addEventListener('click', function () {
        fileElement.remove();
    });

    container.appendChild(fileElement);
}

function saveAsDraft() {
    const formData = new FormData(document.getElementById('claimForm'));

    // You would typically send this to your backend
    console.log('Saving draft:', Object.fromEntries(formData));

    showNotification('Draft saved successfully', 'success');
}

function validateClaimSubmission(event) {
    const totalHoursInput = document.querySelector('input[name="TotalHours"]');
    const hourlyRateInput = document.querySelector('input[name="HourlyRate"]');

    const totalHours = parseFloat(totalHoursInput?.value) || 0;
    const hourlyRate = parseFloat(hourlyRateInput?.value) || 0;

    // Basic validation
    if (totalHours === 0 || hourlyRate === 0) {
        event.preventDefault();
        showNotification('Please fill in all required fields', 'error');
        return;
    }

    // Business rule validation
    if (totalHours > 160) {
        event.preventDefault();
        showNotification('Maximum 160 hours per month allowed', 'error');
        return;
    }

    // Check if it's after the 5th of the month
    const today = new Date();
    if (today.getDate() > 5) {
        if (!confirm('You are submitting after the 5th of the month. This may require manager approval. Continue?')) {
            event.preventDefault();
            return;
        }
    }

    // Show submission confirmation
    showNotification('Submitting claim...', 'info');
}