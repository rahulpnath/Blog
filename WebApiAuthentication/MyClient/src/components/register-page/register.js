define(['knockout', 'text!./register.html', 'app/config'], function(ko, templateMarkup, config) {

    function RegisterPage(params) {
        // TODO: Use Better methods to get values from the query string.
        var tokenSplit = params.token.toString().split('&');
        var token = tokenSplit[0];
        var email = ko.observable(tokenSplit[3].split('=')[1]);
        // Save the token. This can also be stored to the local storage that can be loaded up anytome a page is opened. 
        config.token = token;
        // See if user is already registered
        var userInfoUrl = config.apiBaseUrl + "api/Account/UserInfo";
        $.ajax({
            url: userInfoUrl,
            success: function (data) {
                window.location = "#";
            },
            beforeSend: function (xhr) {
                xhr.setRequestHeader('Authorization', 'Bearer ' + config.token);
            }
        });

        var registerUser = function () {
            $.ajax({
                url: config.apiBaseUrl + 'api/Account/RegisterExternal',
                data: { 'Email': email, 'Name' : email},
                method: 'POST',
                xhrFields: {
                    withCredentials: true
                },
                beforeSend: function(xhr) {
                    xhr.setRequestHeader('Authorization','Bearer ' + token);
                },
                success: function(data) {
                    // Navigate to the user page
                    window.location = "#user";
                },
                failure: function(data) {
                    alert('Registration failed' + data.toString());
                }
            });
        };
        
        return {
            registerUser: registerUser,
            email : email
        }
  }

  // This runs when the component is torn down. Put here any logic necessary to clean up,
  // for example cancelling setTimeouts or disposing Knockout subscriptions/computeds.
  RegisterPage.prototype.dispose = function() { };
  
  return { viewModel: RegisterPage, template: templateMarkup };

});
