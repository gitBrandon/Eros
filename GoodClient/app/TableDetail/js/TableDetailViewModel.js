function TableDetailViewModel(data, fn_modify, fn_delete) {
    var self = this;
    if (data === undefined || data === null) {
        self.Error = ko.observable();
        self.ID = ko.observable(0);
        self.TableHeadID = ko.observable();
        self.TextDetail = ko.observable();
    } else {
        self.Error = ko.observable(data.Error);
        if (data.ID === undefined || data.ID === null) {
            self.ID = ko.observable(0);
        } else {
            self.ID = ko.observable(data.ID);
        }
        self.TableHeadID = ko.observable(data.TableHeadID);
        self.TextDetail = ko.observable(data.TextDetail);
    }

    self.Modify = function() { if (fn_modify !== undefined) fn_modify(self); };
    self.Delete = function() { if (fn_delete !== undefined) fn_delete(self); };
};