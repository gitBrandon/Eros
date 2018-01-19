function TableDetail() {

    var _vm;
    var _TableDetailServiceCaller = new TableDetailServiceCaller();
    var _TableHeadServiceCaller = new TableHeadServiceCaller();

    function Initialise() {
        _vm = new ViewModel();
        setTimeout(function() {
            ko.applyBindings(_vm, $("#TableDetailPage")[0]);
        }, 60);
        GetAll();
        GetTableHeads();
    };

    function ViewModel() {
        var self = this;
        self.MaintObj = ko.observable(new TableDetailViewModel());
        self.IsAdd = ko.observable(true);
        self.TableDetailList = ko.observableArray([]);
        self.TableHeadList = ko.observableArray([]);

        self.SetMaintObj = function(data) {
            for (var prop in data) {
                if (prop != "Modify" && prop != "Delete")
                    self.MaintObj()[prop](data[prop]())
            }
        };
        self.ClearMaintObj = function() {
            for (var prop in self.MaintObj()) {
                if (prop == 'ID') {
                    self.MaintObj()[prop](0);
                } else {
                    self.MaintObj()[prop](null);
                }
            }
        };
        self.Add = function() {
            self.IsAdd(true);
            self.ClearMaintObj();
            $("#create-edit-TableDetail-modal").modal("show");
        };

        self.Save = function(vm, e) {
            if (e.isDefaultPrevented()) {
                // handle the invalid form...
            } else {
                e.preventDefault();
                if (self.IsAdd()) {
                    function callback_Create(response) {
                        self.TableDetailList.push(new TableDetailViewModel(response.TableDetailList[0], Modify, Delete));
                        $("#create-edit-TableDetail-modal").modal("hide");
                        self.ClearMaintObj();
                    }
                    _TableDetailServiceCaller.Create(self.MaintObj, callback_Create)
                } else {

                    function callback_modify(response) {
                        self.TableDetailList([]);
                        GetAll();
                        $("#create-edit-TableDetail-modal").modal("hide");
                        self.ClearMaintObj();
                    }

                    _TableDetailServiceCaller.Modify(self.MaintObj, callback_modify);
                }
            }
        }

        $('#TableDetailForm').validator().on('submit', self.Save);
    };

    function GetAll() {
        _TableDetailServiceCaller.GetAll(function(response) {
            $(response.TableDetailList).each(function(idx, el) {
                _vm.TableDetailList.push(new TableDetailViewModel(el, Modify, Delete));
            });
            setTimeout(() => {
                $('.dropdown-toggle-maint').dropdown();
            }, 200);
        });
    };

    function GetTableHeads() {
    	_TableHeadServiceCaller.GetAll(function(response) {
    		$(response.TableHeadList).each(function(idx, el) {
    			_vm.TableHeadList.push(new TableHeadViewModel(el));
    		})
    	})
    }

    function Modify(self) {
        _vm.SetMaintObj(self);
        _vm.IsAdd(false);
        $("#create-edit-TableDetail-modal").modal("show");
    }

    function Delete(self) {
        _TableDetailServiceCaller.Delete(self.ID(), function(response) {
            _vm.TableDetailList.remove(self);
        });
    }

    return {
        init: function() {
            Initialise();
        }
    }
}

$(document).ready(function() {
    var formObj = new TableDetail();
    formObj.init();
});