function EnsayosCtrl($scope, $http, $rootScope)
{
    $scope.importerResult = [];
    $scope.categoriaId = 0;
    $scope.categorias = [];
    $scope.step = 0;
    $scope.info = null;
    $scope.available = 0;
    $scope.NullErrror = [];
    $scope.NotMatching = [];
    $scope.errorInStep = [];

    function init()
    {
        //get categorias
        $http.get('/Admin/GetCategorias')
        .success(function (data)
        {
            $scope.categorias = data;
            $('#loadingCategory').hide();
        });

        isUploaded();

    }

    $scope.hasErrors = function (step)
    {
        var i = 0
        for (i; i < $scope.errorInStep.length; i++)
            {
            if ($scope.errorInStep[i] == step)
                 return true
            }
        return false
    }
 
    $scope.enableStep = function (step)
    {
        if ($scope.step <= step )
                return true;
        else
            return false;
        }
    
    $scope.categoryChanged = function (categoryId)
    {
        if (categoryId != 0)
            $scope.step = 1;
    }

    function isUploaded()
    {
        $http.post('/Admin/verifyExcelUploaded')
                   .success(function (data)
                   {
                       $scope.available = $scope.step;
                           $scope.info = data.Name;
                   });
    }

    $scope.nextStep = function (nextStep, fileName, categoriaId)
    {
       $scope.step = nextStep;
       switch (nextStep)
       {
          
           case 2: //Verificar archivo subido, y header correcto
               {
                   isUploaded();

                   $http.post('/Admin/verifyExcelHeader?fileName=' + fileName)
                    .success(function (data)
                    {
                        $scope.available = $scope.step;
                        $scope.importerResult = data;
                        if ($scope.importerResult.length != 0)
                        {
                           $scope.errorInStep.push($scope.step);
                            $scope.available = $scope.step - 1;
                        }
                        else
                            $scope.info = 'No hubo errores en las verificaciones realizadas, puede continuar.';

                    });
               }
               break;
           case 3:
               {
                   $http.post('/Admin/ImportExcelToTemporalTable')
                   .success(function (data)
                   {
                       $scope.importerResult = data;
                       if ($scope.importerResult.length == 0)
                       {
                           $scope.info = 'Excel importado correctamente a la tabla temporal.'
                           $scope.available = $scope.step;
                       }
                       else
                       {
                         $scope.errorInStep.push($scope.step);
                           $scope.available = $scope.step - 1;
                       }
                   });
               }
               break;
           case 4:
               {
                   $http.post('/Admin/RunVerifications?categoriaId=' + categoriaId)
                  .success(function (data)
                  {
                      $scope.info = null;
                      $scope.importerResult = data;

                      if ($scope.importerResult.length == 0)
                      {
                          $scope.info = 'Todas las verificaciones pasaron correctamente.'
                          $scope.available = $scope.step;
                      }
                      else
                      {
                          $scope.info = 'Se detectaron los siguientes errores: ' + $scope.importerResult.length;
                          $scope.available = $scope.step;
                         $scope.errorInStep.push($scope.step);

                          for(i=0; i <$scope.importerResult.length; i++ )
                          {
                              if ($scope.importerResult[i].TipoError == 1)
                              {
                                  $scope.NullErrror.push($scope.importerResult[i])
                              }
                              else if ($scope.importerResult[i].TipoError == 2)
                              {
                                  $scope.NotMatching.push($scope.importerResult[i])
                              }
                          }
                      }
                      
                  });
               }
               break;
           case 5:
               {
                   $http.post('/Admin/UpsertEnsayos?categoriaId=' + categoriaId)
                  .success(function (data)
                  {
                      if (data.success)
                      {
                          $scope.info = 'Tabla Ensayos actualizada correctamente.'
                          $scope.importerResult = null;
                      }
                      else
                      {
                          $scope.errorInStep = $scope.errorInStep.push(step);
                          $scope.info = data.description;
                          $scope.importerResult = null;
                      }
                  })

               }
               break;
       }
    }

    init();
}