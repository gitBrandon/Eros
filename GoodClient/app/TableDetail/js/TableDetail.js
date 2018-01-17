function TableDetail() {

    var _vm;
    var _TableDetailServiceCaller = new TableDetailServiceCaller();

    function Initialise() {
        _vm = new ViewModel();
        // var maintObj = new TableDetailViewModel(null);
        setTimeout(function() {
            ko.applyBindings(_vm, $("#TableDetailPage")[0]);
            // ko.mapping.fromJS(maintObj, {}, _vm.MaintObj);
            // _vm.MaintObj.valueHasMutated();
        }, 60);
        GetAll();
    };

    function ViewModel() {
        var self = this;
        /*self.MaintObj = ko.observable({});*/
        self.IsAdd = ko.observable(true);
        self.TableDetailList = ko.observableArray([]);

        self.SetMaintObj = function(data) {
            SetItems(data);
        }

        self.ClearMaintObj = function() {
        }

        self.Add = function(area) {
            self.IsAdd(true);
            ClearMaintObj();
            $("#create-edit-TableDetail-modal").modal("show");
        }

        self.Save = function(vm, e) {
            if (e.isDefaultPrevented()) {
                // handle the invalid form...
            } else {
                e.preventDefault();
                if (self.IsAdd()) {
                    function callback_Create(response) {
                        self.TableDetailList.push(new TableDetailViewModel(response.TableDetailList[0], Modify, Delete));
                        $("#create-edit-TableDetail-modal").modal("hide");
                        _vm.ClearMaintObj();
                    }
                    _TableDetailServiceCaller.Create(self.MaintObj, callback_Create)
                } else {

                    function callback_modify(response) {
                        self.TableDetailList([]);
                        GetAll();
                        $("#create-edit-TableDetail-modal").modal("hide");
                        _vm.ClearMaintObj();
                    }

                    _TableDetailServiceCaller.Modify(self.MaintObj, callback_modify);
                }
            }
        }

        $('#TableDetailForm').validator().on('submit', self.Save);

        function SetItems(data) {
            var x = data;
            for(var prop in x) {
                if (prop !== "Modify" || prop !== "Delete")
                    self[prop] = data[prop];
            }

            console.log(self);
        }

        // SetItems();
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

    function Modify(self) {
        _vm.ClearMaintObj();
        _vm.SetMaintObj(self);
        _vm.IsAdd(false);
        $("#create-edit-TableDetail-modal").modal("show");
    }

    function Delete(self) {
        _TableDetailServiceCaller.Delete(self.ID, function(response) {
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