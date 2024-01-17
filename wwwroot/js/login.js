
const container = document.getElementById('container');
const registerOpenButton = document.getElementById('register');
const loginOpenButton = document.getElementById('login');
const signUpButton = document.getElementById('signUp');
const signInButton = document.getElementById('signIn');

// Event for opening the registration panel.
registerOpenButton.addEventListener('click', () => {
    container.classList.add("active");
});

// Event for opening the login panel.
loginOpenButton.addEventListener('click', () => {
    container.classList.remove("active");
});

// Event click to try login.
signInButton.addEventListener('click', () => signIn());

// Event click to try create an account.
signUpButton.addEventListener('click', () => signUp());

/** Function to handle the sign-in process. */
async function signIn() {
    const singInPanel = document.querySelector('.form-container.sign-in');
    const emailInput = singInPanel.querySelector('.email');
    const passwordInput = singInPanel.querySelector('.password');
    const errorMessage = singInPanel.querySelector('.error-message');

    // Get the trimmed values of email and password.
    const email = emailInput.value.trim();
    const password = passwordInput.value.trim();

    // Validate email and password.
    if (!email || !password) {
        // If either email or password is missing, show an error message.
        errorMessage.classList.add('active');
        errorMessage.textContent = !email ? 'Mail is not entered!' : 'Password is not entered!';

        // Add 'error' class to the corresponding input(s).
        emailInput.classList.toggle('error', !email);
        passwordInput.classList.toggle('error', !password);

        return;
    }

    // If the check is successful, remove the error message and 'error' class from inputs.
    errorMessage.classList.remove('active');
    emailInput.classList.remove('error');
    passwordInput.classList.remove('error');

    // Send a request to the server for user login.
    const response = await fetch('/user/login', {
        method: "POST",
        headers: {
            "Accept": "application/json",
            "Content-Type": "application/json" 
        },
        body: JSON.stringify({
            email: email,
            password: password
        })
    });

    if (response.ok) {
        // If successful, redirect the user to the home page.
        window.location.href = "/";
    }
    else {
         // If unsuccessful, show an error message.
        errorMessage.classList.add('active');
        errorMessage.textContent = 'User not found!';
        emailInput.classList.add('error');
        passwordInput.classList.add('error');
        passwordInput.value = '';
    }
}


/** Function to handle the sign-up process. */
async function signUp() {
    const singUpPanel = document.querySelector('.form-container.sign-up');
    const nameInput = singUpPanel.querySelector('.name');
    const surnameInput = singUpPanel.querySelector('.surname');
    const emailInput = singUpPanel.querySelector('.email');
    const passwordInput = singUpPanel.querySelector('.password');
    const errorMessage = singUpPanel.querySelector('.error-message');

    // Validate each form field and store the result.
    const isNameValid = validateField(nameInput, errorMessage);
    const isSurnameValid = validateField(surnameInput, errorMessage);
    const isEmailValid = validateField(emailInput, errorMessage);
    const isPasswordValid = validateField(passwordInput, errorMessage);

     // Check if any field is invalid.
    if (!(isNameValid && isSurnameValid && isEmailValid && isPasswordValid)) {
        // If any field is invalid, show an error message and return.
        errorMessage.classList.add('active');
        errorMessage.textContent = 'Not all data is entered!';
        return;
    }

    // If the check is successful, remove the error message.
    errorMessage.classList.remove('active');

    // Send a request to the server for user registration.
    const response = await fetch('/user/registration', {
        method: "POST",
        headers: {
            "Accept": "application/json",
            "Content-Type": "application/json"
        },
        body: JSON.stringify({
            name: nameInput.value.trim(),
            surname: surnameInput.value.trim(),
            email: emailInput.value.trim(),
            password: passwordInput.value.trim()
        })
    });

    if (response.ok) {
        // If successful, redirect the user to the home page.
        window.location.href = "/";
    }
    else {
        errorMessage.classList.add('active');
        errorMessage.textContent = 'Incorrect data. Try to chenge something!';
    }
}

// Function to validate a form field.
function validateField(input, errorMessage) {
    // Get the trimmed value of the field.
    const value = input.value.trim();
     // Check if the value is empty.
    if (!value) {
        input.classList.add('error');
        return false;
    } else {
        input.classList.remove('error');
        return true;
    }
}