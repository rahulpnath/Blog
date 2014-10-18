define(['knockout', 'text!./user.html', 'app/config'], function(ko, templateMarkup, config) {

    function User(params) {
        var userInfoUrl = config.apiBaseUrl + "api/Account/UserInfo";
        $.getJSON(userInfoUrl)
            .done(function(data) {
            alert(data);
        });
    }

  // This runs when the component is torn down. Put here any logic necessary to clean up,
  // for example cancelling setTimeouts or disposing Knockout subscriptions/computeds.
  User.prototype.dispose = function() { };
  
  return { viewModel: User, template: templateMarkup };

});
