import os
import numpy as np
import tensorflow as tf
from dotenv import load_dotenv

load_dotenv()
base_dir = os.getenv('BASE_DIR')
models_dir = os.path.join(str(base_dir), 'models')

def make_dataset(dataframe, encoder, batch_size, is_train=True):
    arr_labels = np.array(dataframe["rating"]).reshape(-1, 1)
    encoded_y = encoder.transform(arr_labels)

    dataset = tf.data.Dataset.from_tensor_slices(
        (dataframe["text"].values, encoded_y)
    )
    dataset = dataset.shuffle(batch_size * 10) if is_train else dataset
    return dataset.batch(batch_size)
