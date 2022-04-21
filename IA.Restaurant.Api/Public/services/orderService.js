application.factory('ordersService',function($http, DOMAIN){
  let baseUrl = DOMAIN.BASE_URL;       
  console.log(baseUrl);
  return {
      get: function() {
        let config = {};
        return $http.get(baseUrl + '/Orders?queryfilter=[{"PropertyName": "IdStatus","Value": 1, "Comparison": 0}]', config)
      },
      patch: function(id, body) {
        let config = {};
        return $http.patch(baseUrl + '/Orders/'+id,body, config);
      },
      getByIdStatus: function(id) {
        let config = {};
        return $http.get(baseUrl + '/Orders?queryfilter=[{"PropertyName": "IdStatus","Value": '+id+', "Comparison": 0}]', config);
      },
  };
}
);