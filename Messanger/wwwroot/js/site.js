// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.
$(".login-sign-info-item .login-sign-btn").click(function(){
    $(".login-sign-container").toggleClass("log-in");
});
$(".container-form .btn").click(function(){
    $(".login-sign-container").addClass("active");
});
$(document).ready(function () {
    $("form").submit(function (event) {
        var isValid = true;
        // Перевірка для входу
        if ($(this).hasClass("log-in")) {
            var email = $(this).find("input[name='email']").val().trim().toLowerCase(); // Нормалізуємо до нижнього регістру
            var password = $(this).find("input[name='password']").val().trim();

            if (email.length === 0) {
                isValid = false;
                $("#log-in-error-email").html("Це поле є обов'язковим для заповнення");
            } else {
                if (!isValidEmail(email)) {
                    isValid = false;
                    $("#log-in-error-email").html("Неправильний формат електронної пошти");
                } else {
                    $("#log-in-error-email").html(""); // Очистити повідомлення про помилку
                }
            }

            if (password.length === 0) {
                isValid = false;
                $("#log-in-error-password").html("Це поле є обов'язковим для заповнення");
            } else {
                $("#log-in-error-password").html(""); // Очистити повідомлення про помилку
            }
        }

        // Перевірка для реєстрації
        if ($(this).hasClass("sign-up")) {
            var username = $(this).find("input[name='name']").val().trim();
            var email = $(this).find("input[name='email']").val().trim().toLowerCase(); // Нормалізуємо до нижнього регістру
            var password = $(this).find("input[name='password']").val().trim();

            if (username.length === 0) {
                isValid = false;
                $("#sign-up-error-name").html("Це поле є обов'язковим для заповнення");
            } else if (username.length <= 2 || username.length >= 15) {
                isValid = false;
                $("#sign-up-error-name").html("Ім'я має містити від 3 до 15 символів");
            }else {
                $("#sign-up-error-name").html(""); // Очистити повідомлення про помилку
            }

            if (email.length === 0) {
                isValid = false;
                $("#sign-up-error-email").html("Це поле є обов'язковим для заповнення");
            } else {
                if (!isValidEmail(email)) {
                    isValid = false;
                    $("#sign-up-error-email").html("Неправильний формат електронної пошти");
                } else {
                    $("#sign-up-error-email").html(""); // Очистити повідомлення про помилку
                }
            }

            if (password.length === 0) {
                isValid = false;
                $("#sign-up-error-password").html("Це поле є обов'язковим для заповнення");
            } else if (password.length <= 6) {
                isValid = false;
                $("#sign-up-error-password").html("Пароль має містити від 6 до 50 символів");
            }else {
                $("#sign-up-error-password").html(""); // Очистити повідомлення про помилку
            }
        }
        if (!isValid) {
            event.preventDefault(); // Зупиняємо відправку форми
        }
    });

    // Очищення повідомлень про помилки при введенні даних
    $("input[name='name'], input[name='password'], input[name='email']").keyup(function () {
        var fieldName = $(this).attr("name");
        $("#" + fieldName + "-error").html(""); // Очистити повідомлення про помилку
    });
});
function isValidEmail(email) {
    // Регулярний вираз для перевірки формату електронної пошти
    var emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailRegex.test(email);
}

// Write your JavaScript code.
