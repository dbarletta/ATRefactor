
APP.controller('ProductoLugaresCtrl', function ($scope, $rootScope, $resource, $http) {
    $scope.Nombre = $rootScope.producto.nombre;

    var Product = $resource('/api/Productos/:id', { id: '@id' }, null);

    $scope.saveLugares = function () {
        listaChk = _.where($scope.productoLugares, { isChecked: true });
        $http.post('/Admin/SaveProductoLugares', { lista: listaChk, productId: $rootScope.producto.id })
             .success(function (data) {
                 $scope.productoLugares = data;
                 $('.fancybox-close').click();
              });
    }

    //initialize
    $scope.productoLugares = Product.query({ productoId: $rootScope.producto.id });

});