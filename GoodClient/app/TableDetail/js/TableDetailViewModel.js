function TableDetailViewModel(data, fn_modify, fn_delete,TableHeadList) {    var self = this;    if(data === undefined || data === null) {        self.Error = ko.observable();        self.ID = ko.observable(0);        self.TableHeadID = ko.observable();        self.TextDetail = ko.observable();    }     else {        self.Error = ko.observable(data.Error);        if(data.ID === undefined || data.ID === null) {            self.ID = ko.observable(0);        } else {            self.ID = ko.observable(data.ID);        }        self.TableHeadID = ko.observable(data.TableHeadID);        self.TextDetail = ko.observable(data.TextDetail);    }if (TableHeadList !==undefined) {self.TableHead_Friendly =  ko.computed(function() {var result = TableHeadList().filter(function(obj) {return obj.ID() == self.TableHeadID();});if (result.length > 0) {   return result[0].Name();} else {   return self.TableHeadID();}});} else {self.TableHead_Friendly =  ko.observable();}    self.Modify = function() {        if(fn_modify !== undefined)          fn_modify(self);    };    self.Delete = function() {        if(fn_delete !== undefined)          fn_delete(self);    };};