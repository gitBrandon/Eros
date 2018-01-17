function TableDetailViewModel(data, fn_modify, fn_delete) {
    var self = this;
    if (data === undefined || data === null) {
        self.Error = "ko.observable()";
        self.ID = "ko.observable()";
        self.TableHeadID = "ko.observable()";
        self.TextDetail = "ko.observable()";
    } else {
        self.Error = data.Error;
        self.ID = data.ID;
        self.TableHeadID = data.TableHeadID;
        self.TextDetail = data.TextDetail;
    }
    self.Modify = function() { if (fn_modify !== undefined) fn_modify(self); };
    self.Delete = function() { if (fn_delete !== undefined) fn_delete(self); };
};