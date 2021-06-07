function validateEmail(email, errName) {
    var pattern = /^\b[A-Z0-9._%-]+@[A-Z0-9.-]+\.[A-Z]{2,4}\b$/i
    var arr = document.getElementById(errName);
    if (!pattern.test(email)) {
        arr.innerHTML = "Invalid email";
        return false;
    } else {
        arr.innerHTML = "";
        return true;
    }
}

function validateContact(value, errName) {
    
    var arr = document.getElementById(errName);
    if (value.substring(0, 2) != "01" || value.length >= 12 || value.length<10) {
        arr.innerHTML = "Invalid mobile number";
        return false;
    } else {
        arr.innerHTML = "";
        return true;
    }
}