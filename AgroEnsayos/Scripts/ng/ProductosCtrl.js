
APP.controller('ProductosCtrl', function ($scope, $rootScope, $compile, $resource) {

    var Category = $resource('/api/Categorias', null, null);
    var Company = $resource('/api/Empresas', null, null);
    var Product = $resource('/api/Productos/:id', { id: '@id' }, null);

    $scope.getProductos = function () {
        $('#loadingCategory').show();
        $scope.productos = Product.query({ categoryId: $scope.categoriaId }, function () {
            $('#loadingCategory').hide();
        });
    }

    $scope.sortBy = function (col) {
        $scope.orderby = col;
        $scope.reverse = !$scope.reverse;
    }

    $scope.addProduct = function () {
        $scope.selectedProduct = new Product({ id: 0 })
    }

    $scope.cancelAdd = function () {
        $scope.selectedProduct = null;
    }

    $scope.editProduct = function (p) {
        $scope.selectedProduct = p;
        $scope.productCopy = angular.copy(p);
    }

    $scope.cancelEdit = function (p) {
        var pos = $scope.productos.indexOf(p);
        $scope.productos[pos] = $scope.productCopy;
        $scope.selectedProduct = null;
        $scope.productCopy = null;
    }

    $scope.saveProduct = function (isNew) {
        var p = $scope.selectedProduct;
        p.categoria = _.findWhere($scope.categorias, { id: p.categoriaId }).nombre;
        p.empresa = _.findWhere($scope.empresas, { id: p.empresaId }).nombre;
        p.$save(function () {
            if (isNew) {
                $scope.productos.push($scope.selectedProduct);
            }
            $scope.selectedProduct = null;
        });
    }

    $scope.promptDelete = function (p) {
        $('html,body').animate({ scrollTop: 0 }, 'fast');
        $scope.productToDelete = p;
    }

    $scope.deleteProduct = function (p) {
        $scope.productToDelete.$delete(function () {
            var ix = $scope.productos.indexOf($scope.productToDelete);
            $scope.productos.splice(ix, 1);
            $scope.productToDelete = null;
        });
    }

    $scope.showEditProduct = function (p) {
        if ($scope.editingProduct != null && $scope.editingProduct.Id == p.Id)
            return true;

        return false;
    }

    $scope.showReadProduct = function (p) {
        if ($scope.editingProduct == null || $scope.editingProduct.Id != p.Id)
            return true;

        return false;
    }

    $scope.editZones = function (p) {
        $rootScope.producto = p;
        $('#popupZones .loading').show();
        $('#popupZones .container').empty();
        $.fancybox.open([{
                href: '#popupZones',
                minHeight: 200,
                scrolling: 'no'
            }]);

        $("#popupZones .container").load("/Admin/ProductoLugares", function () {
            $('#popupZones .loading').hide();
            var lugaresHtml = $('#popupZones .container').html();
            var compiledHtml = $compile(lugaresHtml)($scope);
            $('#popupZones .container').html(compiledHtml);
        });
    }

    $scope.editAttributes = function (p) {
        $rootScope.producto = p;

        $('#popupAttributes .loading').show();
        $('#popupAttributes .container').empty();
        $.fancybox.open([
            {
                href: '#popupAttributes',
                minHeight: 200,
                scrolling: 'no'
            }]);

        $("#popupAttributes .container").load("/Admin/ProductoAtributos", function () {
            $('#popupAttributes .loading').hide();

            var atributosHtml = $('#popupAttributes .container').html();
            var compiledHtml = $compile(atributosHtml)($scope);
            $('#popupAttributes .container').html(compiledHtml);
        });
    }

    //initialize
    $scope.orderby = 'id';
    $scope.reverse = false;
    $scope.categoriaId = 4;
    $scope.categorias = Category.query();
    $scope.empresas = Company.query(function () { console.log($scope.empresas); });
    $scope.getProductos();
});

