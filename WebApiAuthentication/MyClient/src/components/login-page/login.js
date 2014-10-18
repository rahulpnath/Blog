define(["knockout", "text!./login.html", "app/config"], function (ko, loginTemplate, config) {

    function LoginViewModel(route) {
        var
            providers = ko.observable([]),
            baseUrl = config.apiBaseUrl, 
            returnUrl=config.clientUrl,  
            init = function () {
                // logout if already login
                var logoutUrl = config.baseUrl + "api/Account/Logout";
                $.getJSON(logoutUrl);

                // load the providers from the Web Api ExternalLogins
                var url = baseUrl  + "api/Account/ExternalLogins?returnUrl=" + returnUrl;
                $.getJSON(url)
                    .done(function(response) {
                        providers(response);
                    });
            },
            loginExternal = function(provider) {
                location.href = "http://localhost:19731" + provider.Url;
            };


        init();

        return {
            providers: providers,
            loginExternal: loginExternal
        };
    }

    return { viewModel: LoginViewModel, template: loginTemplate };

});
