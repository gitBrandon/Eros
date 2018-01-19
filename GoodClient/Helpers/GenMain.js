function GenMain() {
    var self = this;
    var _fw = new FileWriter();
    var _configHelper = new ConfigHelper();
    var _includeTableAsID;
    var _linkHelper = new LinkHelper();

    self.Gen = function(el, response) {
        var item = response[el.Name + "List"][0];
        var linkDataArr = _linkHelper.GetLinkData(item);

        var strMainData = `
			function ${el.Name}() {

				var _vm;
				var _${el.Name}ServiceCaller = new ${el.Name}ServiceCaller();
				`;

        $(linkDataArr).each(function(idx, tableName) {
            strMainData += `var _${tableName}ServiceCaller = new ${tableName}ServiceCaller();`;
        })

        strMainData += `function Initialise() {
					_vm = new ViewModel();
					setTimeout(function() {
						ko.applyBindings(_vm, $("#${el.Name}Page")[0]);
					}, 60);
					GetAll();`;

        $(linkDataArr).each(function(idx, tableName) {
            strMainData += `GetAll${tableName}();`;
        });

        strMainData += `};`;

        strMainData += `function ViewModel() {
					var self = this;
					self.MaintObj = ko.observable(new ${el.Name}ViewModel());
					self.IsAdd = ko.observable(true);
					self.${el.Name}List = ko.observableArray([]);`;

        $(linkDataArr).each(function(idx, tableName) {
            strMainData += `self.${tableName}List = ko.observableArray([]);`;
        });

        strMainData += `self.SetMaintObj = function(data) {
			            for (var prop in data) {
			                if (prop != "Modify" && prop != "Delete")
			                    self.MaintObj()[prop](data[prop]())
			            }
			        };`;

        if (_includeTableAsID) {
            strMainData += `self.ClearMaintObj = function() {
				            for (var prop in self.MaintObj()) {
				            	if(prop == '${el.Name}ID') {
				                	self.MaintObj()[prop](0);
				            	}
				            	else {
				            		self.MaintObj()[prop](null);	
				            	}
				            }
				        };`;
        } else {
            strMainData += `self.ClearMaintObj = function() {
				            for (var prop in self.MaintObj()) {
				            	if(prop == 'ID') {
				                	self.MaintObj()[prop](0);
				            	}
				            	else {
				            		self.MaintObj()[prop](null);	
				            	}
				            }
				        };`;
        }

        strMainData += `self.Add = function () {
            			self.IsAdd(true);
			            self.ClearMaintObj();
            			$("#create-edit-${el.Name}-modal").modal("show");
        			};

			        self.Save = function (vm, e) {
			            if (e.isDefaultPrevented()) {
			                // handle the invalid form...
			            } else {
			                e.preventDefault();            
			                if (self.IsAdd()) {
			                    function callback_Create(response) {`;
        if (linkDataArr.length <= 0) {
            strMainData += `_vm.${ el.Name }List.push(new ${ el.Name }ViewModel(response.${el.Name}List[0], Modify, Delete));`;
        } else {

            strMainData += `_vm.${ el.Name }List.push(new ${ el.Name }ViewModel(response.${el.Name}List[0], Modify, Delete,`;
            $(linkDataArr).each(function(idx, tableName) {
                if (idx == linkDataArr.length - 1) {
                    strMainData += "_vm." + tableName + "List));";
                } else {
                    strMainData += "_vm." + tableName + "List, ";
                }
            })
        }
        //self.${el.Name}List.push(new ${el.Name}ViewModel(response.${el.Name}List[0], Modify, Delete));
        strMainData += `$("#create-edit-${el.Name}-modal").modal("hide");
			                        self.ClearMaintObj();
			                    }
			                    _${el.Name}ServiceCaller.Create(self.MaintObj, callback_Create)
			                } else {

			                    function callback_modify(response) {
			                        self.${el.Name}List([]);
			                        GetAll();
			                        $("#create-edit-${el.Name}-modal").modal("hide");
			                        self.ClearMaintObj();
			                    }

			                    _${el.Name}ServiceCaller.Modify(self.MaintObj, callback_modify);
			                }
			            }
			        }

			        $('#${el.Name}Form').validator().on('submit', self.Save);
				};

				function GetAll() {
					_${el.Name}ServiceCaller.GetAll(function(response) {
						$(response.${el.Name}List).each(function(idx, el) {`;
        if (linkDataArr.length <= 0) {
            strMainData += `_vm.${ el.Name }List.push(new ${ el.Name }ViewModel(el, Modify, Delete));`;
        } else {

            strMainData += `_vm.${ el.Name }List.push(new ${ el.Name }ViewModel(el, Modify, Delete,`;
            $(linkDataArr).each(function(idx, tableName) {
                if (idx == linkDataArr.length - 1) {
                    strMainData += "_vm." + tableName + "List));";
                } else {
                    strMainData += "_vm." + tableName + "List, ";
                }
            })
        }
        strMainData += `});
				        setTimeout(() => {
	            			$('.dropdown-toggle-maint').dropdown();
	    				}, 200);
					});
				};`;

        $(linkDataArr).each(function(idx, tableName) {
            strMainData += `function GetAll${tableName}() {
            					_${tableName}ServiceCaller.GetAll(function(response) {
            						$(response.${tableName}List).each(function(idx, el) {
            							_vm.${tableName}List.push(new ${tableName}ViewModel(el));
            						});
							        setTimeout(() => {
	            						$('.dropdown-toggle-maint').dropdown();
    								}, 200);
            					});
            				}
            `;
        });

        strMainData += `function Modify(self) {
					_vm.SetMaintObj(self);
					_vm.IsAdd(false);
					$("#create-edit-${el.Name}-modal").modal("show");
				}

				function Delete(self) {
					_${el.Name}ServiceCaller.Delete(self.ID(), function(response) {
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

    function GetIncludeTableAsID() {
        _configHelper.GetIncludeTableAsID(function(response) {
            _includeTableAsID = response;
        })
    }

    GetIncludeTableAsID();
}