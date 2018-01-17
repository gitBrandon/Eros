
			function TableHead() {

				var _vm;
				var _TableHeadServiceCaller = new TableHeadServiceCaller();

				function Initialise() {
					_vm = new ViewModel();
					setTimeout(function() {
						ko.applyBindings(_vm);
					}, 60);
					GetAll();
				};

				function ViewModel() {
					var self = this;
					self.MaintObj = new TableHeadViewModel(null);
					self.IsAdd = ko.observable(true);
					self.TableHeadList = ko.observableArray([]);

					var SetMaintObj = function(data) {
						self.MaintObj = new TableHeadViewModel(data);
					}

					var ClearMaintObj = function() {
						self.MaintObj = new TableHeadViewModel(null);
					}

			        _self.Add = function (area) {
            			_self.IsAdd(true);
			            ClearMaintObj();
            			$("#create-edit-TableHead-modal").modal("show");
        			}

			        self.Save = function (e) {
			            if (e.isDefaultPrevented()) {
			                // handle the invalid form...
			            } else {
			                e.preventDefault();            
			                if (_self.IsAdd()) {
			                    function callback_Create(response) {
			                        self.TableHeadList.push(new TableHeadViewModel(response.TableHeadList[0], Modify, Delete));
			                        $("#create-edit-TableHead-modal").modal("hide");
			                        ClearMaintObj();
			                    }
			                    _TableHeadServiceCaller.Create(self.MaintObj, callback_Create)
			                } else {

			                    function callback_modify(response) {
			                        self.TableHeadList([]);
			                        GetAll();
			                        $("#create-edit-TableHead-modal").modal("hide");
			                        ClearMaintObj();
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
					ClearMaintObj();
					$("#create-edit-TableHead-modal").modal("show");
				}

				function Delete(self) {
					_TableHeadServiceCaller.Delete(self.ID, function(response) {
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
		