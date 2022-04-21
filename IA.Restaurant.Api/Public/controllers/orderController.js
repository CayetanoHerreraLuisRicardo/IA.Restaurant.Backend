application.controller('orderController', function ($scope, ordersService, statusService, $filter, toaster) {
    $scope.lstStatus = statusService.get();
    $scope.statusSelected = 0;
    $scope.lstOrders = [];
    $scope.changeStatus = function (id) {
        getOrdersByIdStatus(id);
    }
    $scope.getNextStatus = function (idStatus) {
        return $scope.lstStatus[idStatus].name;
    }
    $scope.cancel = function (idStatus, idOrder) {
        if (idStatus < 3) {
            let body = { idOrder, idStatus: 5 };
            ordersService.patch(idOrder, body).success(function (data) {
                console.log(data);
                getOrdersByIdStatus($scope.statusSelected);
                toaster.success({ title: 'success', body: 'order #' + idOrder +' was successfully canceled' });
            }).error(function (e) {
                console.error(e);
            });
        } else {
            toaster.warning('warning', 'this order can no longer be canceled');
        }
    }
    $scope.updateStatus = function (idStatus, idOrder) {
        if (idStatus < 4) {
            let idSiguiente = $scope.lstStatus[idStatus].idStatus;
            let body = { idOrder, idStatus: idSiguiente };
            ordersService.patch(idOrder, body).success(function (data) {
                console.log(data);
                getOrdersByIdStatus($scope.statusSelected);
                toaster.success({ title: 'success', body: 'the status of order #' + idOrder +' was successfully updated' });
            }).error(function (e) {
                console.error(e);
            });
        } else {
            toaster.warning('warning', 'his order can no longer be changed in status');
        }
    }
    function getOrdersByIdStatus(id) {
        $scope.statusSelected = id;
        ordersService.getByIdStatus(id).success(function (data) {
            $scope.lstOrders = data;
        }).error(function (e) {
            toaster.error('error', 'error trying to query data');
        });
    }
    getOrdersByIdStatus(1);

    $scope.clear = function () {
        toaster.clear();
    };
    function notify (order) {
        toaster.success({ title: 'new order', body: 'a new order #' + order.idOrder+' has just been added!' });
    }
    $scope.init = function () {
        const connection = new signalR.HubConnectionBuilder()
            .withUrl('/hubs/orders')
            .build();

        connection.on('notify', (order) => {
            console.log(order);
            if ($scope.statusSelected === 1) {
                $scope.lstOrders.splice(0, 0, order)
            }
            notify(order);
            toaster.pop('warning', 'title', 'Bill');
        });

        async function start() {
            try {
                await connection.start();
                console.log('SignalR Connected.');
            } catch (err) {
                console.log(err);
                setTimeout(start, 5000);
            }
        }

        connection.onclose(async () => {
            await start();
        });

        // Start the connection.
        start();
    }
});