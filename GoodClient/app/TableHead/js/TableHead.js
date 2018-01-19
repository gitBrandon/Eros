
			function TableHead() {

				var _vm;
				var _TableHeadServiceCaller = new TableHeadServiceCaller();

				function Initialise() {
					_vm = new ViewModel();
					setTimeout(function() {
						ko.applyBindings(_vm, $("#TableHeadPage")[0]);
					}, 60);
					GetAll();
				};

				function ViewModel() {
					var self = this;
					self.MaintObj = ko.observable(new TableHeadViewModel());
					self.IsAdd = ko.observable(true);
					self.TableHeadList = ko.observableArray([]);

			        self.SetMaintObj = function(data) {
			            for (var prop in data) {
			                if (prop != "Modify" && prop != "Delete")
			                    self.MaintObj()[prop](data[prop]())
			            }
			        };self.ClearMaintObj = function() {
				            for (var prop in self.MaintObj()) {
				            	if(prop == 'ID') {
				                	self.MaintObj()[prop](0);
				            	}
				            	else {
				            		self.MaintObj()[prop](null);	
				            	}
				            }
				        };self.Add = function () {
            			self.IsAdd(true);
			            self.ClearMaintObj();
            			$("#create-edit-TableHead-modal").modal("show");
        			};

			        self.Save = function (vm, e) {
			            if (e.isDefaultPrevented()) {
			                // handle the invalid form...
			            } else {
			                e.preventDefault();            
			                if (self.IsAdd()) {
			                    function callback_Create(response) {
			                        self.TableHeadList.push(new TableHeadViewModel(response.TableHeadList[0], Modify, Delete));
			                        $("#create-edit-TableHead-modal").modal("hide");
			                        self.ClearMaintObj();
			                    }
			                    _TableHeadServiceCaller.Create(self.MaintObj, callback_Create)
			                } else {

			                    function callback_modify(response) {
			                        self.TableHeadList([]);
			                        GetAll();
			                        $("#create-edit-TableHead-modal").modal("hide");
			                        self.ClearMaintObj();
			                    }

			                    _TableHeadServiceCaller.Modify(self.MaintObj, callback_modify);
			                }
			            }
			        }

			        $('#TableHeadForm').validator().on('submit', self.Save);
				};

				function GetAll() {
					_TableHeadServiceCaller.GetAll(function(response) {
						$(response.TableHeadList).each(function(idx, el) {
							_vm.TableHeadList.push(new TableHeadViewModel(el, Modify, Delete));
						});
				        setTimeout(() => {
	            			$('.dropdown-toggle-maint').dropdown();
	    				}, 200);
					});
				};

				function Modify(self) {
					_vm.SetMaintObj(self);
					_vm.IsAdd(false);
					$("#create-edit-TableHead-modal").modal("show");
				}

				function Delete(self) {
					_TableHeadServiceCaller.Delete(self.ID(), function(response) {
						_vm.TableHeadList.remove(self);
					});
				}

				return {
					init: function() {
						Initialise();
					}
				}
			}

			$(document).ready(function() {
				var formObj = new TableHead();
				formObj.init();
			});
		