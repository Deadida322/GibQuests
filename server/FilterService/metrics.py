from sklearn.metrics import (accuracy_score, mean_absolute_error as mae,
                             mean_absolute_percentage_error as mape,
                             mean_squared_error as mse,
                             root_mean_squared_error as rmse)
def print_metrics(y_test, predict):
    print('accuracy_score', accuracy_score(y_test, predict))
    print('mae: ', mae(y_test, predict))
    print('mape: ', mape(y_test, predict))
    print('mse: ', mse(y_test, predict))
    print('rmse: ', rmse(y_test, predict))