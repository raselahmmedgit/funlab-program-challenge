var SignIn = function () {
    var initSignIn = function () {

        $("#btnSignInSubmit").on('click', function () {
            _submit();
        });

    };

    var _submit = function () {

        let dataObj = {
            UserEmail: $("#UserEmail").val(),
            UserPassword: $("#UserPassword").val()
        }

        //let authorization = ('Bearer ' + localStorage.getItem('token'));

        $.ajax({
            type: 'POST',
            url: '/Account/Login',
            data: JSON.stringify(dataObj),

            dataType: 'json',
            contentType: 'application/json',
            //headers: { "Authorization": authorization },
            beforeSend: function () {
                App.LoaderShow();
            },
            success: function (result) {
                debugger;
                //console.log(result);
                if (result != undefined || result != null) {

                    if (result.success == true) {
                        //console.log(result.message);
                        $('.message').addClass('text-success');
                        $('.message').html('');
                        $('.message').html(result.message);
                        window.location.href = (window.location.origin + result.redirectUrl);
                    }
                    else {
                        //console.log(result.message);
                        $('.message').addClass('text-danger');
                        $('.message').html('');
                        $('.message').html(result.message);
                    }

                    //window.location.reload();
                }
                //check null
            },
            complete: function (xhr, status) {
                App.LoaderHide();
            },
            error: function (req, status, error) {
                App.LoaderHide();
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

        let dataObj = {
            UserName: $("#UserName").val(),
            UserEmail: $("#UserEmail").val(),
            UserPassword: $("#UserPassword").val(),
            UserConfirmPassword: $("#UserConfirmPassword").val(),
            CardNumber: $("#CardNumber").val(),
            CardExpiration: $("#CardExpiration").val(),
            CardCvc: $("#CardCvc").val(),
            CardCountry: $("#CardCountry").val()
        }

        $.ajax({
            type: "POST",
            url: "/Account/Register",
            data: JSON.stringify(dataObj),

            dataType: 'json',
            contentType: 'application/json',
            beforeSend: function () {
                App.LoaderShow();
            },
            success: function (result) {
                debugger;
                //console.log(result);
                if (result != undefined || result != null) {

                    if (result.success == true) {
                        //console.log(result.message);
                        $('.message').addClass('text-success');
                        $('.message').html('');
                        $('.message').html(result.message);
                        window.location.href = (window.location.origin + result.redirectUrl);
                    }
                    else {
                        //console.log(result.message);
                        $('.message').html('');
                        $('.message').html(result.message);
                    }

                    //window.location.reload();
                }
                //check null
            },
            complete: function (xhr, status) {
                App.LoaderHide();
            },
            error: function (req, status, error) {
                App.LoaderHide();
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
