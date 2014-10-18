define(["knockout", "text!./home.html", 'app/config'], function(ko, homeTemplate, config) {

    function HomeViewModel(route) {
        var name = ko.observable();
        var registered = ko.observable();
        if (config.token != null) {
            var userInfoUrl = config.apiBaseUrl + "api/Account/UserInfo";
            $.ajax({
                url: userInfoUrl,
                success: function(data) {
                    name(data.Email);
                    registered(data.HasRegistered);
                },
                beforeSend: function(xhr) {
                    xhr.setRequestHeader('Authorization', 'Bearer ' + config.token);
                }
            });
        } else {
            // redirect to login page
            window.location = "#login";
        }

        return {
            name: name,
            registered: registered
        };
    }

  HomeViewModel.prototype.doSomething = function() {
    this.message('You invoked doSomething() on the viewmodel.');
  };

  return { viewModel: HomeViewModel, template: homeTemplate };

});
