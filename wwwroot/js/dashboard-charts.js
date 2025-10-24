// Dashboard Charts and Analytics for Caim Management System

// Initialize all charts
function initializeManagerCharts() {
    initializeClaimsTrendChart();
    initializeProgrammeDistributionChart();
    initializeApprovalMetricsChart();
}

// Claims Trend Chart
function initializeClaimsTrendChart() {
    const ctx = document.getElementById('claimsTrendChart');
    if (!ctx) return;

    new Chart(ctx, {
        type: 'line',
        data: {
            labels: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep'],
            datasets: [
                {
                    label: 'Claims Submitted',
                    data: [45, 52, 48, 61, 55, 58, 65, 62, 45],
                    borderColor: '#059669',
                    backgroundColor: 'rgba(5, 150, 105, 0.1)',
                    tension: 0.4,
                    fill: true
                },
                {
                    label: 'Claims Approved',
                    data: [38, 45, 42, 52, 48, 50, 58, 55, 40],
                    borderColor: '#10b981',
                    backgroundColor: 'rgba(16, 185, 129, 0.1)',
                    tension: 0.4,
                    fill: true
                }
            ]
        },
        options: {
            responsive: true,
            plugins: {
                legend: {
                    position: 'top',
                },
                title: {
                    display: false
                }
            },
            scales: {
                y: {
                    beginAtZero: true,
                    title: {
                        display: true,
                        text: 'Number of Claims'
                    }
                }
            }
        }
    });
}

// Programme Distribution Chart
function initializeProgrammeDistributionChart() {
    const ctx = document.getElementById('programmeDistributionChart');
    if (!ctx) return;

    new Chart(ctx, {
        type: 'doughnut',
        data: {
            labels: ['Computer Science', 'IT', 'Software Eng', 'Data Science', 'Cybersecurity'],
            datasets: [{
                data: [35, 25, 20, 12, 8],
                backgroundColor: [
                    '#059669',
                    '#10b981',
                    '#34d399',
                    '#6ee7b7',
                    '#a7f3d0'
                ],
                borderWidth: 2,
                borderColor: '#ffffff'
            }]
        },
        options: {
            responsive: true,
            plugins: {
                legend: {
                    position: 'bottom'
                }
            }
        }
    });
}

// Approval Metrics Chart
function initializeApprovalMetricsChart() {
    const ctx = document.getElementById('approvalMetricsChart');
    if (!ctx) return;

    new Chart(ctx, {
        type: 'bar',
        data: {
            labels: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun'],
            datasets: [
                {
                    label: 'Approval Rate (%)',
                    data: [88, 92, 85, 90, 94, 89],
                    backgroundColor: '#059669',
                    yAxisID: 'y'
                },
                {
                    label: 'Avg Processing (Days)',
                    data: [3.2, 2.8, 3.5, 2.9, 2.4, 2.7],
                    backgroundColor: '#f59e0b',
                    yAxisID: 'y1'
                }
            ]
        },
        options: {
            responsive: true,
            interaction: {
                mode: 'index',
                intersect: false,
            },
            scales: {
                y: {
                    type: 'linear',
                    display: true,
                    position: 'left',
                    title: {
                        display: true,
                        text: 'Approval Rate %'
                    },
                    min: 80,
                    max: 100
                },
                y1: {
                    type: 'linear',
                    display: true,
                    position: 'right',
                    title: {
                        display: true,
                        text: 'Processing Days'
                    },
                    min: 0,
                    max: 5,
                    grid: {
                        drawOnChartArea: false,
                    },
                }
            }
        }
    });
}

// Lecturer Performance Chart
function initializeLecturerPerformanceChart() {
    const ctx = document.getElementById('lecturerPerformanceChart');
    if (!ctx) return;

    new Chart(ctx, {
        type: 'radar',
        data: {
            labels: ['Submission Time', 'Accuracy', 'Documentation', 'Approval Rate', 'Amount Accuracy'],
            datasets: [{
                label: 'Dr. John Smith',
                data: [85, 92, 88, 95, 90],
                fill: true,
                backgroundColor: 'rgba(5, 150, 105, 0.2)',
                borderColor: '#059669',
                pointBackgroundColor: '#059669',
                pointBorderColor: '#fff',
                pointHoverBackgroundColor: '#fff',
                pointHoverBorderColor: '#059669'
            }, {
                label: 'Department Average',
                data: [75, 80, 70, 85, 78],
                fill: true,
                backgroundColor: 'rgba(156, 163, 175, 0.2)',
                borderColor: '#9ca3af',
                pointBackgroundColor: '#9ca3af',
                pointBorderColor: '#fff',
                pointHoverBackgroundColor: '#fff',
                pointHoverBorderColor: '#9ca3af'
            }]
        },
        options: {
            elements: {
                line: {
                    borderWidth: 2
                }
            },
            scales: {
                r: {
                    angleLines: {
                        display: true
                    },
                    suggestedMin: 50,
                    suggestedMax: 100
                }
            }
        }
    });
}

// Budget Utilization Chart
function initializeBudgetUtilizationChart() {
    const ctx = document.getElementById('budgetUtilizationChart');
    if (!ctx) return;

    new Chart(ctx, {
        type: 'bar',
        data: {
            labels: ['Computer Science', 'IT', 'Software Eng', 'Data Science', 'Cybersecurity'],
            datasets: [{
                label: 'Budget Used',
                data: [85, 72, 90, 65, 45],
                backgroundColor: '#059669'
            }, {
                label: 'Budget Remaining',
                data: [15, 28, 10, 35, 55],
                backgroundColor: '#d1fae5'
            }]
        },
        options: {
            responsive: true,
            scales: {
                x: {
                    stacked: true,
                },
                y: {
                    stacked: true,
                    title: {
                        display: true,
                        text: 'Percentage %'
                    },
                    min: 0,
                    max: 100
                }
            }
        }
    });
}

// Real-time Metrics Update
function updateRealTimeMetrics() {
    // Simulate real-time data updates
    const metrics = {
        pendingClaims: Math.floor(Math.random() * 10) + 5,
        approvedToday: Math.floor(Math.random() * 8) + 3,
        totalAmount: (Math.random() * 50000 + 200000).toFixed(2)
    };

    // Update dashboard elements
    document.querySelectorAll('[data-metric="pending"]').forEach(el => {
        el.textContent = metrics.pendingClaims;
    });

    document.querySelectorAll('[data-metric="approved"]').forEach(el => {
        el.textContent = metrics.approvedToday;
    });

    document.querySelectorAll('[data-metric="amount"]').forEach(el => {
        el.textContent = 'R ' + metrics.totalAmount;
    });
}

// Initialize all charts when DOM is loaded
document.addEventListener('DOMContentLoaded', function () {
    // Initialize charts based on current page
    const path = window.location.pathname;

    if (path.includes('/Manager/') || path.includes('/Admin/')) {
        initializeManagerCharts();
    }

    if (path.includes('/Coordinator/')) {
        initializeApprovalMetricsChart();
    }

    // Set up real-time updates for dashboards
    if (path.includes('/Dashboard')) {
        // Update every 30 seconds
        setInterval(updateRealTimeMetrics, 30000);

        // Initial update
        setTimeout(updateRealTimeMetrics, 1000);
    }
});

// Export functionality for charts
function exportChartAsImage(chartId, filename) {
    const chartCanvas = document.getElementById(chartId);
    if (!chartCanvas) return;

    const image = chartCanvas.toDataURL('image/png');
    const link = document.createElement('a');
    link.download = filename + '.png';
    link.href = image;
    link.click();
}

// Data export functions
function exportChartData(chartId, format = 'csv') {
    const chart = Chart.getChart(chartId);
    if (!chart) return;

    let data = '';
    const labels = chart.data.labels;
    const datasets = chart.data.datasets;

    if (format === 'csv') {
        // Create CSV header
        data = 'Category,' + datasets.map(ds => ds.label).join(',') + '\n';

        // Add data rows
        labels.forEach((label, index) => {
            data += label + ',' + datasets.map(ds => ds.data[index]).join(',') + '\n';
        });
    }

    // Download the data
    const blob = new Blob([data], { type: 'text/csv' });
    const url = URL.createObjectURL(blob);
    const link = document.createElement('a');
    link.download = 'chart-data-' + chartId + '.csv';
    link.href = url;
    link.click();
}