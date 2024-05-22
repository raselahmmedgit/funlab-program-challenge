var SignIn = function () {
    var initSignIn = function () {

        $("#btnSignInSubmit").on('click', function () {
            _submit();
        });

    };

    var _submit = function () {

        var userEmail = $('#UserEmail').val();
        var userPassword = $('#UserPassword').val();

        $.ajax({
            type: "POST",
            url: "/Account/Login",
            data: { UserEmail: userEmail, UserPassword: userPassword },
            dataType: "html",
            beforeSend: function () {
                console.log('beforeSend');
            },
            success: function (result) {
                debugger;
                if (result != undefined || result != null) {
                    
                }
            },
            error: function (req, status, error) {
                console.log(error);
            }
        });

    };

    return {
        InitSignIn: initSignIn
    };
}();


var SignUp = function () {
    var initSignUp = function () {

        $("#btnSignUpSubmit").on('click', function () {
            _submit();
        });

    };

    var _submit = function () {

        var userName = $('#UserName').val();
        var userEmail = $('#UserEmail').val();
        //var userPassword = $('#UserPassword').val();
        //var userConfirmPassword = $('#UserConfirmPassword').val();
        var cardNumber = $('#CardNumber').val();
        var cardExpiration = $('#CardExpiration').val();
        var cardCvc = $('#CardCvc').val();
        var cardCountry = $('#CardCountry').val();

        $.ajax({
            type: "POST",
            url: "/Account/Register",
            data: { UserName: userName, UserEmail: userEmail, CardNumber: cardNumber, CardExpiration: cardExpiration, CardCvc: cardCvc, CardCountry: cardCountry },
            dataType: "html",
            beforeSend: function () {
                console.log('beforeSend');
            },
            success: function (result) {
                debugger;
                if (result != undefined || result != null) {

                }
            },
            error: function (req, status, error) {
                console.log(error);
            }
        });
    };

    return {
        InitSignUp: initSignUp
    };
}();

var WebCalculator = function () {
    var initWebCalculator = function () {

        const display = document.querySelector(".web-calc-display");
        const buttons = document.querySelectorAll("button");
        const specialChars = ["%", "*", "/", "-", "+", "="];
        let output = "";

        //Define function to calculate based on button clicked.
        const calculate = (btnValue) => {
            display.focus();
            if (btnValue === "=" && output !== "") {
                //If output has '%', replace with '/100' before evaluating.
                output = eval(output.replace("%", "/100"));
            } else if (btnValue === "AC") {
                output = "";
            } else if (btnValue === "DEL") {
                //If DEL button is clicked, remove the last character from the output.
                output = output.toString().slice(0, -1);
            } else if (btnValue === "MC") {
                output = "";
            } else if (btnValue === "MR") {
                output = "";
            } else if (btnValue === "M+") {
                output = "";
            } else if (btnValue === "M-") {
                output = "";
            } else {
                //If output is empty and button is specialChars then return
                if (output === "" && specialChars.includes(btnValue)) return;
                output += btnValue;
            }
            display.value = output;
        };

        //Add event listener to buttons, call calculate() on click.
        buttons.forEach((button) => {
            //Button click listener calls calculate() with dataset value as argument.
            button.addEventListener("click", (e) => calculate(e.target.dataset.value));
        });

    };

    return {
        InitWebCalculator: initWebCalculator
    };
}();
