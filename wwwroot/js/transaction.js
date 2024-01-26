
const searchInput = document.getElementById('search');
const transactionElements = document.querySelectorAll('.scroll-space table tbody tr');
const typeSelect = document.querySelector('.sortbar select.type');
const categorySelect = document.querySelector('.sortbar select.category');

// Variables for sorting.
let searchTerm = "";
let choosedCategory = "default";
let choosedType = "any";

// The 'input' event on the 'searchInput' element.
searchInput.addEventListener('input', () => {
    // Get the trimmed and lowercase version of the search term.
    searchTerm = searchInput.value.trim().toLowerCase();
    updateTransactions();
});

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

    // Set the default option as selected in the 'categotySelect' dropdown.
    categorySelect.querySelector('option.default').selected = true;

    // Update transactions list.
    choosedType = selectedValue;
    choosedCategory = "default";
    updateTransactions();
});

// If choose category, the transaction list will be updated.
categorySelect.addEventListener('change', function () {
    choosedCategory = this.value;
    updateTransactions();
});

// Function of updating the list on transaction according to sorting.
function updateTransactions() {
    // Take each row of the list and check whether it corresponds to the sorting.
    for (transactionElement of transactionElements) {
        // Display all transactions.
        transactionElement.style.display = "table-row";
        const typeIsIncome = transactionElement.querySelector('.income') ? true : false;

        // Type sorting.
        if (choosedType === "income") {
            if (!typeIsIncome) transactionElement.style.display = "none";
        }
        else if (choosedType === "expense") {
            if (typeIsIncome) transactionElement.style.display = "none";
        }    

        // Category sorting.
        const categoryText = transactionElement.querySelector('.category').textContent;
        const isMatch = choosedCategory === "default" || categoryText === choosedCategory;
        if (!isMatch) transactionElement.style.display = "none";

        // Search term sorting.
        const title = transactionElement.querySelector('.title').textContent.toLowerCase();
        if (!title.includes(searchTerm)) transactionElement.style.display = "none";
    }    
}