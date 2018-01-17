function GenMain() {
    var self = this;
    var _fw = new FileWriter();

    self.Gen = function(el, response) {

        var strMainData = `
			function ${el.Name}() {

				var _vm;
				var _${el.Name}ServiceCaller = new ${el.Name}ServiceCaller();

				function Initialise() {
					_vm = new ViewModel();
					setTimeout(function() {
						ko.applyBindings(_vm, $("#{el.Name}Page")[0]);
					}, 60);
					GetAll();
				};

				function ViewModel() {
					var self = this;
					self.MaintObj = new ${el.Name}ViewModel(null);
					self.IsAdd = ko.observable(true);
					self.${el.Name}List = ko.observableArray([]);

					var SetMaintObj = function(data) {
						self.MaintObj = new ${el.Name}ViewModel(data);
					}

					var ClearMaintObj = function() {
						self.MaintObj = new ${el.Name}ViewModel(null);
					}

			        self.Add = function (area) {
            			self.IsAdd(true);
			            ClearMaintObj();
            			$("#create-edit-${el.Name}-modal").modal("show");
        			}

			        self.Save = function (vm, e) {
			            if (e.isDefaultPrevented()) {
			                // handle the invalid form...
			            } else {
			                e.preventDefault();            
			                if (self.IsAdd()) {
			                    function callback_Create(response) {
			                        self.${el.Name}List.push(new ${el.Name}ViewModel(response.${el.Name}List[0], Modify, Delete));
			                        $("#create-edit-${el.Name}-modal").modal("hide");
			                        ClearMaintObj();
			                    }
			                    _${el.Name}ServiceCaller.Create(self.MaintObj, callback_Create)
			                } else {

			                    function callback_modify(response) {
			                        self.${el.Name}List([]);
			                        GetAll();
			                        $("#create-edit-${el.Name}-modal").modal("hide");
			                        ClearMaintObj();
			                    }

			                    _${el.Name}ServiceCaller.Modify(self.MaintObj, callback_modify);
			                }
			            }
			        }

			        $('#${el.Name}Form').validator().on('submit', self.Save);
				};

				function GetAll() {
					_${el.Name}ServiceCaller.GetAll(function(response) {
						$(response.${el.Name}List).each(function(idx, el) {
							_vm.${el.Name}List.push(new ${el.Name}ViewModel(el, Modify, Delete));
						});
				        setTimeout(() => {
	            			$('.dropdown-toggle-maint').dropdown();
	    				}, 200);
					});
				};

				function Modify(self) {
					_vm.SetMaintObj(self);
					_vm.IsAdd(false);
					ClearMaintObj();
					$("#create-edit-${el.Name}-modal").modal("show");
				}

				function Delete(self) {
					_${el.Name}ServiceCaller.Delete(self.ID, function(response) {
						_vm.${el.Name}List.remove(self);
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


        _fw.Write(el.Name + "/js", el.Name, strMainData);
    }
}