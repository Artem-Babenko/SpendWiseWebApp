
const searchInput = document.getElementById('search');
const transactionElements = document.querySelectorAll('table tbody tr');
const typeSelect = document.querySelector('.sortbar .type');
const categorySelect = document.querySelector('.sortbar .category');

// The 'input' event on the 'searchInput' element.
searchInput.addEventListener('input', () => {
    // Get the trimmed and lowercase version of the search term.
    const searchTerm = searchInput.value.trim().toLowerCase();

    // Loop through each transaction element in the table.
    for (const element of transactionElements) {
        // Get the lowercase version of the text content of the 'title' element within the current transaction.
        const title = element.querySelector('.title').textContent.toLowerCase();
        // Check if the title includes the search term.
        if (title.includes(searchTerm))
            element.style.display = "table-row";
        else
            element.style.display = "none";
    }
});

// When selecting a transaction type,
// the list of possible categories for selection is updated.
typeSelect.addEventListener('change', function () {
    var selectedValue = this.value;

    // Loop through each option in the 'categotySelect' dropdown.
    for (const option of categorySelect.options) {
        // Hide the option and deselect it.
        option.style.display = "none";
        option.selected = false;

        // Check if the selected type is 'any' or if the option belongs to the selected type.
        if (selectedValue === "any" || option.classList.contains(selectedValue)) {
            // Display the option if it matches the selected type.
            option.style.display = "block";
        }
    }

    // Loop through each transaction element in the table.
    for (const transactionElement of transactionElements) {
        // Check if the selected type is 'any' or if the transaction has the selected category.
        const isMatchingCategory = selectedValue === "any" ||
            transactionElement.querySelector('.' + selectedValue);
        // Set the display property of the transaction element based on the category match.
        transactionElement.style.display = isMatchingCategory ? "table-row" : "none";
    }

    // Set the default option as selected in the 'categotySelect' dropdown.
    categorySelect.querySelector('option.default').selected = true;
});

// When selecting a transaction category,
// the list of transaction is updated.
categorySelect.addEventListener('change', function () {
    var selectedValue = this.value;

    // Loop through each transaction element.
    for (const transactionElement of transactionElements) {
        // Get the category text content of the current transaction element.
        const categoryText = transactionElement.querySelector('.category').textContent;
        // Check if the selected value is "default" or matches the category of the transaction.
        const isMatch = selectedValue === "default" || categoryText === selectedValue;
        // Set the display style based on whether it's a match or not.
        transactionElement.style.display = isMatch ? "table-row" : "none";
    }
});