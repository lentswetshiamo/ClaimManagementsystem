// Claim auto-calculation and validation

$(document).ready(function() {
    // Auto-calculate total amount
    function calculateTotal() {
        var hoursWorked = parseFloat($('#HoursWorked').val()) || 0;
        var hourlyRate = parseFloat($('#HourlyRate').val()) || 0;
        var totalAmount = hoursWorked * hourlyRate;
        
        $('#TotalAmount').val(totalAmount.toFixed(2));
        $('#totalAmountDisplay').text('R ' + totalAmount.toFixed(2));
    }

    // Bind calculation to input changes
    $('#HoursWorked, #HourlyRate').on('input', function() {
        calculateTotal();
        validateClaim();
    });

    // Real-time validation
    function validateClaim() {
        var isValid = true;
        var errors = [];

        // Clear previous errors
        $('.validation-error').remove();
        $('.form-control').removeClass('is-invalid');

        var hoursWorked = parseFloat($('#HoursWorked').val()) || 0;
        var hourlyRate = parseFloat($('#HourlyRate').val()) || 0;
        var claimType = $('#ClaimType').val();
        var description = $('#Description').val();

        // Validate hours worked
        if (hoursWorked <= 0) {
            isValid = false;
            errors.push('Hours worked must be greater than zero');
            $('#HoursWorked').addClass('is-invalid');
            $('<div class="validation-error text-danger small">Hours worked must be greater than zero</div>')
                .insertAfter('#HoursWorked');
        } else if (hoursWorked > 1000) {
            isValid = false;
            errors.push('Hours worked cannot exceed 1000');
            $('#HoursWorked').addClass('is-invalid');
            $('<div class="validation-error text-danger small">Hours worked cannot exceed 1000</div>')
                .insertAfter('#HoursWorked');
        }

        // Validate hourly rate
        if (hourlyRate <= 0) {
            isValid = false;
            errors.push('Hourly rate must be greater than zero');
            $('#HourlyRate').addClass('is-invalid');
            $('<div class="validation-error text-danger small">Hourly rate must be greater than zero</div>')
                .insertAfter('#HourlyRate');
        } else if (hourlyRate > 10000) {
            isValid = false;
            errors.push('Hourly rate cannot exceed 10000');
            $('#HourlyRate').addClass('is-invalid');
            $('<div class="validation-error text-danger small">Hourly rate cannot exceed 10000</div>')
                .insertAfter('#HourlyRate');
        }

        // Validate claim type
        if (!claimType || claimType.trim() === '') {
            isValid = false;
            errors.push('Claim type is required');
            $('#ClaimType').addClass('is-invalid');
        }

        // Validate description
        if (!description || description.trim().length < 10) {
            isValid = false;
            errors.push('Description must be at least 10 characters');
            $('#Description').addClass('is-invalid');
            $('<div class="validation-error text-danger small">Description must be at least 10 characters</div>')
                .insertAfter('#Description');
        }

        // Enable/disable submit button
        $('#submitClaimBtn').prop('disabled', !isValid);

        return isValid;
    }

    // Validate on form submission
    $('#claimForm').on('submit', function(e) {
        if (!validateClaim()) {
            e.preventDefault();
            alert('Please fix all validation errors before submitting.');
            return false;
        }
    });

    // Validate on input changes
    $('#ClaimType, #Description').on('input', validateClaim);

    // Initialize on page load
    if ($('#HoursWorked').length > 0) {
        calculateTotal();
        validateClaim();
    }

    // Initialize DataTables for claim tables
    if ($('#claimsTable').length > 0) {
        $('#claimsTable').DataTable({
            order: [[0, 'desc']], // Sort by ID descending
            pageLength: 10,
            lengthMenu: [[10, 25, 50, -1], [10, 25, 50, "All"]],
            dom: 'Bfrtip',
            buttons: ['copy', 'csv', 'excel', 'pdf', 'print']
        });
    }

    // Bulk action checkbox handling
    $('#selectAll').on('change', function() {
        $('.claim-checkbox').prop('checked', $(this).prop('checked'));
        updateBulkActionButtons();
    });

    $('.claim-checkbox').on('change', function() {
        updateBulkActionButtons();
    });

    function updateBulkActionButtons() {
        var selectedCount = $('.claim-checkbox:checked').length;
        $('#bulkApproveBtn, #bulkRejectBtn').prop('disabled', selectedCount === 0);
        $('#selectedCount').text(selectedCount);
    }

    // Bulk approve
    $('#bulkApproveBtn').on('click', function() {
        var claimIds = [];
        $('.claim-checkbox:checked').each(function() {
            claimIds.push($(this).val());
        });

        if (confirm('Are you sure you want to approve ' + claimIds.length + ' claim(s)?')) {
            $('#bulkClaimIds').val(claimIds.join(','));
            $('#bulkApproveForm').submit();
        }
    });

    // Bulk reject
    $('#bulkRejectBtn').on('click', function() {
        var claimIds = [];
        $('.claim-checkbox:checked').each(function() {
            claimIds.push($(this).val());
        });

        var reason = prompt('Please enter rejection reason:');
        if (reason && reason.trim() !== '') {
            $('#bulkClaimIds').val(claimIds.join(','));
            $('#bulkRejectionReason').val(reason);
            $('#bulkRejectForm').submit();
        }
    });

    // Document upload validation
    $('#documentUpload').on('change', function() {
        var file = this.files[0];
        if (file) {
            var allowedExtensions = ['pdf', 'doc', 'docx', 'txt', 'jpg', 'jpeg', 'png'];
            var extension = file.name.split('.').pop().toLowerCase();
            var maxSize = 5 * 1024 * 1024; // 5MB

            if (!allowedExtensions.includes(extension)) {
                alert('Invalid file type. Allowed types: PDF, DOC, DOCX, TXT, JPG, PNG');
                $(this).val('');
                return;
            }

            if (file.size > maxSize) {
                alert('File size exceeds 5MB limit');
                $(this).val('');
                return;
            }

            $('#fileInfo').text(file.name + ' (' + (file.size / 1024).toFixed(2) + ' KB)');
        }
    });
});
