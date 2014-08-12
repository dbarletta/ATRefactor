
APP.controller('ProductoAtributosCtrl', function ($scope, $http, $rootScope) {

    $scope.getProductoAtributos = function (producto) {
        $http.post('/Admin/GetProductoAtributos', { producto: producto })
        .success(function (data) {
            $scope.productoAtributos = data;
        });
    }

    $scope.editAttribute = function (attribute) {
        $scope.atributoEditing = attribute;
        $scope.atributoEditingCopy = angular.copy(attribute);
    }

    $scope.cancelAttributeEdition = function (attr) {
        $scope.atributoEditing = null;


        for (var i = 0, len = $scope.productoAtributos.Atributos.length; i < len; i++) {
            if ($scope.productoAtributos.Atributos[i].Id == attr.Id) {
                $scope.productoAtributos.Atributos[i] = $scope.atributoEditingCopy;
            }
        }
    }

    $scope.savePartialAttributeEdition = function () {
        $scope.atributoEditing = null;
    }

    $scope.saveAttributeEdition = function () {
        $http.post('/Admin/SaveProductoAtributos', { Atributos: $scope.productoAtributos.Atributos, ProductoId: $scope.productoAtributos.Id })
        .success(function () {
            $scope.atributoEditing = null;
            $('.fancybox-close').click();
        });
    }

    $scope.showEditAttr = function (attr) {
        if ($scope.atributoEditing != null && $scope.atributoEditing.Id == attr.Id)
            return true;

        return false;
    }

    $scope.showReadAttr = function (attr) {
        if ($scope.atributoEditing == null || $scope.atributoEditing.Id != attr.Id)
            return true;

        return false;
    }


    //initialization
    $scope.getProductoAtributos($rootScope.producto);

});
