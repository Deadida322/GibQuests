from sklearn.metrics import (accuracy_score, mean_absolute_error as mae,
                             mean_absolute_percentage_error as mape,
                             mean_squared_error as mse,
                             root_mean_squared_error as rmse)
def print_metrics(y_test, predict):
    print('accuracy_score', round(accuracy_score(y_test, predict), 2))
    print('mae: ', round(mae(y_test, predict), 2))
    print('mape: ', round(mape(y_test, predict),2))
    print('mse: ', round(mse(y_test, predict),2))
    print('rmse: ', round(rmse(y_test, predict),2))