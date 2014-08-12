
APP.controller('AtributosCtrl', function ($scope, $resource) {
    
    var Category = $resource('/api/Categorias/:id', { id: '@id' }, null);
    var Attribute = $resource('/api/Atributos/:id', { id: '@id' }, null);


    $scope.getDataTypeName = function (attr) {
        var obj = _.findWhere($scope.dataTypes, { id: attr.tipoDato });
        return obj.desc;
    }

    $scope.sortBy = function (col) {
        $scope.orderby = col;
        $scope.reverse = !$scope.reverse;
    }

    $scope.editAttribute = function (p) {
        $scope.selectedAttribute = p;
        $scope.editCopy = angular.copy(p);
    }

    $scope.cancelEdit = function (p) {
        var pos = $scope.attributes.indexOf(p);
        $scope.attributes[pos] = $scope.editCopy;
        $scope.selectedAttribute = null;
        $scope.editCopy = null;
    }

    $scope.addAttribute = function () {
        $scope.selectedAttribute = new Attribute({ id: 0 });
        console.log($scope.selectedAttribute);
    }

    $scope.cancelNew = function () {
        $scope.selectedAttribute = null;
    }

    $scope.saveAttribute = function (isNew) {
        $scope.selectedAttribute.$save(function () {
            if (isNew) {
                $scope.attributes.push($scope.selectedAttribute);
            }
            $scope.selectedAttribute = null;
        });
    }

    $scope.promptDelete = function (p) {
        $('html,body').animate({ scrollTop: 0 }, 'fast');
        $scope.attributeToDelete = p;
    }

    $scope.deleteAttribute = function () {
        $scope.attributeToDelete.$delete(function () {
            var ix = $scope.attributes.indexOf($scope.attributeToDelete);
            $scope.attributes.splice(ix, 1);
            $scope.attributeToDelete = null;
        });
    }

    $scope.refreshAttributes = function () {
        $scope.attributes = null;
        var prods = Attribute.query(function () {
            $scope.attributes = prods;
        });
    }


    //initialization
    $scope.orderby = 'id';
    $scope.reverse = false;
    $scope.categorias = Category.query();
    $scope.attributes = Attribute.query();
    $scope.dataTypes = [{ id: 1, desc: 'Texto' },
                        { id: 2, desc: 'Numerico' },
                        { id: 3, desc: 'Sí/No' },
                        { id: 4, desc: 'Porcentaje' }];
});





