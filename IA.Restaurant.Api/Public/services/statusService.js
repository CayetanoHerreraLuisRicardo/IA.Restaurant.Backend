application.factory('statusService',function(){
  return {
      get: function() {
        return [{idStatus:1,name:'Pending'},{idStatus:2,name:'In Process'},{idStatus:3, name:'Completed'},{idStatus:4, name:'Delivered'},{idStatus:5, name:'Canceled'}]
      },
  };
}
);