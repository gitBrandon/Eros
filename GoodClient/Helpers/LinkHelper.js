function LinkHelper() {

    var self = this;
    var _configHelper = new ConfigHelper();
    var _entityList;

    self.GetLinkData  = function(serviceResult, currentEntityIgnore) {
        var response = [];

        for (var prop in serviceResult) {
            var result = _entityList.filter(function(el) {
                if (prop.indexOf(el) > -1 && prop.indexOf(currentEntityIgnore) < 0) {
                    return el;
                }
            })
            if (result != null && result.length > 0) {
                response.push(result[0]);
            }
        }

        return response;
    }

    function GetItems() {
        _configHelper.GetItems(function(response) {
            _entityList = response;
        })
    }

    GetItems();
}

/*
 purpose: 
  * To return the linked table list
  * To return the linked table friendly property
  * to return the Linked table service caller

*/