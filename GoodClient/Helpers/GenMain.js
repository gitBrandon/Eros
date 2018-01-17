function GenMain() {
    var self = this;
    var _fw = new FileWriter();

    self.Gen = function(el, response) {

        var strMainData = `
			function ${el.Name}() {

				var _vm;
				var _${el.Name}ServiceCaller = new ${el.Name}ServiceCaller();

				function Initialise() {

				}

				function ViewModel() {
					var self = this;
					self.${el.Name}List = ko.observableArray([]);
				}

				function GetAll() {
					_${el.Name}ServiceCaller.GetAll(function(response) {
						$(response.${el.Name}List).each(function(idx, el) {
							_vm.${el.Name}List.push(new ${el.Name}ViewModel(el));
						})
					});
				}

				return {
					init: function() {
						Initialise();
					}
				}
			}

			$(document).ready(function() {
				var formObj = new ${el.Name}();
				formObj.init();
			});
		`;


        _fw.Write(el.Name + "/js", "" + el.Name + ".js", strMainData);
    }
}