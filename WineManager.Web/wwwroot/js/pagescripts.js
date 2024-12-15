// Toggles all checkboxes in the table
function toggleAllCheckboxes(selectAllCheckbox, checkboxName) {
    const checkboxes = document.querySelectorAll(`input[name="${checkboxName}"]`);
    checkboxes.forEach(checkbox => {
        checkbox.checked = selectAllCheckbox.checked;
    });
    toggleRemoveButton(checkboxName, 'removeButton');
}

// Enables or disables the "Remove Selected" button
function toggleRemoveButton(checkboxName, buttonId) {
    const checkboxes = document.querySelectorAll(`input[name="${checkboxName}"]:checked`);
    const removeButton = document.getElementById(buttonId);
    removeButton.disabled = checkboxes.length === 0;
}

// Initializes event listeners for a table
function initializeTable(checkboxName, selectAllId, buttonId) {
    // Attach toggleRemoveButton to each checkbox change
    document.querySelectorAll(`input[name="${checkboxName}"]`).forEach(checkbox => {
        checkbox.addEventListener("change", () => toggleRemoveButton(checkboxName, buttonId));
    });

    // Optional: Attach toggleRemoveButton to "Select All" checkbox
    const selectAllCheckbox = document.getElementById(selectAllId);
    if (selectAllCheckbox) {
        selectAllCheckbox.addEventListener("change", () => toggleAllCheckboxes(selectAllCheckbox, checkboxName));
    }
}

// Validates a form and enables/disables the submit button
function validateForm(fieldIds, submitButtonId) {
    const allValid = fieldIds.every(id => {
        const field = document.getElementById(id);
        if (!field) {
            console.error(`Field with ID "${id}" not found`); // Log if a field is not found
            return false; // If any field is missing, consider the form invalid
        }
        return field.checkValidity(); // Check validity of the field
    });

    const submitButton = document.getElementById(submitButtonId);
    if (submitButton) {
        submitButton.disabled = !allValid; // Enable/Disable submit button based on validity
    }
}

// Initializes form validation
function initializeFormValidation(fieldIds, submitButtonId) {
    fieldIds.forEach(id => {
        const field = document.getElementById(id);
        if (field) {
            field.addEventListener('input', () => validateForm(fieldIds, submitButtonId)); // Add event listener for input events
        }
    });

    // Initial validation check
    validateForm(fieldIds, submitButtonId);
}

