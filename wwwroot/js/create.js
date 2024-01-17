
const typeSelect = document.querySelector('form .type');
const categotySelect = document.querySelector('form .category');

// When selecting a transaction type, 
// the list of possible categories for selection is updated.
typeSelect.addEventListener('change', function () {
    // Get the selected value from the dropdown.
    var selectedValue = this.value;

    // Loop through each option in the category select dropdown.
    for (const option of categotySelect.options) {
        option.style.display = "none";
        option.selected = false;

        // Check if the option matches the selected transaction type.
        if (selectedValue === "any" || option.classList.contains(selectedValue)) {
            option.style.display = "block";
        }
    }

    // Set the default option as selected in the category select dropdown.
    categotySelect.querySelector('option.default').selected = true;
});
