document.addEventListener("DOMContentLoaded", function () {
function ValidateUser(event) {
    event.preventDefault(); // Prevent default form submission

    let isValid = true;

    const city = document.getElementById("txtCity").value.trim();
    const state = document.getElementById("txtState").value.trim();
    const country = document.getElementById("ddlCountry").value;
    const gender = document.querySelector('input[name="gender"]:checked');
    const dob = document.getElementById("txtDob").value;
    const profileImage = document.getElementById("txtImage").value;

    const cityError = document.getElementById("cityError");
    const stateError = document.getElementById("StateError");
    const countryError = document.getElementById("CountryError");
    const genderError = document.getElementById("GenderError");
    const dobError = document.getElementById("DobError");
    const profileError = document.getElementById("ProfileError");

    cityError.textContent = "";
    stateError.textContent = "";
    countryError.textContent = "";
    genderError.textContent = "";
    dobError.textContent = "";
    profileError.textContent = "";

    // Validate City
    if (!city) {
        cityError.textContent = "City is required.";
        isValid = false;
    }

    // Validate State
    if (!state) {
        stateError.textContent = "State is required.";
        isValid = false;
    }

    // Validate Country
    if (country === "-1") {
        countryError.textContent = "Please select a country.";
        isValid = false;
    }

    // Validate Gender
    if (!gender) {
        genderError.textContent = "Gender is required.";
        isValid = false;
    }

    // Validate Date of Birth
    if (!dob) {
        dobError.textContent = "Date of Birth is required.";
        isValid = false;
    } else {
        const dobDate = new Date(dob);
        const today = new Date();
        if (dobDate >= today) {
            dobError.textContent = "Date of Birth cannot be today or a future date.";
            isValid = false;
        }
    }

    // Validate Profile Image
    if (!profileImage) {
        profileError.textContent = "Please upload a profile image.";
        isValid = false;
    }

    return isValid;
}

function GetQueryParam(name) {
    const urlParams = new URLSearchParams(window.location.search);
    return urlParams.get(name);
}

function FetchUserId() {
   
    const userId = GetQueryParam("userId");

    if (userId) {
        console.log(`Fetched UserId: ${userId}`);
        return userId;
    } else {
        alert("UserId not found in the URL.");
        return null; 
    }
}

const userId = FetchUserId();

function UserData(event) {
    if (!ValidateUser(event)) {
        alert("Validation not successful");
        return;
    }

    const city = document.getElementById("txtCity").value.trim();
    const state = document.getElementById("txtState").value.trim();
    const country = document.getElementById("ddlCountry").value;
    const genderElement = document.querySelector('input[name="gender"]:checked');
    const gender = genderElement ? genderElement.value : null;
    const dob = document.getElementById("txtDob").value;
    const imageInput = document.getElementById("txtImage");
    const profileImageFile = imageInput.files[0];

    if (!gender) {
        alert("Please select a gender.");
        return;
    }

    if (!profileImageFile) {
        alert("Please upload a profile image.");
        return;
    }

    // Convert image to Base64 and submit data
    const reader = new FileReader();
    reader.onload = function () {
        const base64String = reader.result.split(',')[1];

        $.ajax({
            type: "POST",
            url: "WebService/UserRegistration.asmx/UserDetails",
            data: JSON.stringify({
                userId: userId,
                city: city,
                state: state,
                country: country,
                gender:gender,
                dob: dob,
                profileImage: base64String
            }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                console.log("Response:", response);
                if (response.d > 0) {
                    alert("User Data Saved Successfully");
                    
                } else {
                    alert("Failed to Save User Data");
                }
            },
            error: function (xhr, status, error) {
                console.error("Error: " + error);
                console.error("Response Text: " + xhr.responseText);
            }
        });
    };

    reader.readAsDataURL(profileImageFile);
}

// Event listener for form submission
    document.getElementById("userForm").addEventListener("submit", function (event) {
        event.preventDefault(); 
        UserData(event);
    });
});