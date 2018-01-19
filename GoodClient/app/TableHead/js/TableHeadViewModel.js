function TableHeadViewModel(data, fn_modify, fn_delete) {
    var self = this;
    if (data === undefined || data === null) {
        self.CreatedDateTime = ko.observable();
        self.Error = ko.observable();
        self.ID = ko.observable(0);
        self.Name = ko.observable();
        self.Status = ko.observable();
    } else {
        self.CreatedDateTime = ko.observable(data.CreatedDateTime);
        self.Error = ko.observable(data.Error);
        if (data.ID === undefined || data.ID === null) { self.ID = ko.observable(0); } else { self.ID = ko.observable(data.ID); } self.Name = ko.observable(data.Name);
        self.Status = ko.observable(data.Status);
    }
    self.Modify = function() { if (fn_modify !== undefined) fn_modify(self); };
    self.Delete = function() { if (fn_delete !== undefined) fn_delete(self); };
};