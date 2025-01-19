import codecs
import pandas as pd
from dotenv import load_dotenv
import os

from preprocessing import preprocess_train_text, lemmed_and_clear_stop_words, clear_short_words, check_most_common_words

load_dotenv()
directory = os.getenv('DARA_DIR')
base_dir= os.getenv('BASE_DIR')
export_dir = 'process-data-1'
def get_texts():
    data = pd.read_csv(os.path.join(str(directory), 'description.csv'), delimiter=';',
                names=['file', 'name', 'author', 'rating', 'description'])

    previews = []
    # for row in data[['file', 'rating']].head(text_len).itertuples():
    for row in data[['file', 'rating']].itertuples():
        file_path = os.path.join(str(directory), 'previews', row.file)
        if (os.path.isfile(file_path)):
            # fileObj = codecs.open(file_path, "r", "utf-8")
            fileObj = codecs.open(file_path, "r", "cp1251")
            text = fileObj.read()
            rating = row.rating
            if (rating == 5 or rating == 3 or rating == 8):
                rating = 6
            if(rating == 11 or rating == 12):
                rating = 12
            previews.append((text, rating))
            fileObj.close()
    return previews

def get_texts_2(text_len = 3000):
    data = pd.read_csv(os.path.join(str(directory), 'description.csv'), delimiter=';',
                names=['file', 'name', 'author', 'rating', 'description'])
    previews = []
    for row in data[['file', 'author', 'rating']].head(text_len).itertuples():
        print(row)
        fileObj = codecs.open(os.path.join(str(directory), 'previews', row.file), "r", "utf_8_sig" )
        text = fileObj.read()
        rating = row.rating
        author = row.author

        text = preprocess_train_text(text, author)
        words = text.split()
        lemmed_words = lemmed_and_clear_stop_words(words)
        not_short_words = clear_short_words(lemmed_words, min_freq=3)

        previews.append((''.join(not_short_words), rating))
        fileObj.close()
    return previews

def write_file():
    data = pd.read_csv(os.path.join(str(directory), 'description.csv'), delimiter=';',
                names=['file', 'name', 'author', 'rating', 'description'])
    for row in data[['file', 'author']].itertuples():
        file_path = os.path.join(str(directory), 'previews', row.file)
        export_file_path = os.path.join(export_dir, 'previews', row.file)
        print(row.file, 'read')
        if(not os.path.isfile(export_file_path)):
            file_obj = codecs.open(file_path, "r", "utf_8_sig")
            text = file_obj.read()
            author = row.author

            text = preprocess_train_text(text, author)
            lemmed_words = lemmed_and_clear_stop_words(text)
            not_short_words = clear_short_words(lemmed_words, min_freq=1)

            with open(export_file_path, 'w') as file:
                text_to_write = ' '.join(not_short_words)
                file.write(text_to_write)
            file_obj.close()
            print(row.file, 'write')

def get_texts_3(skip = 0, take = 0):
    data = pd.read_csv(os.path.join(str(directory), 'description.csv'), delimiter=';',
                names=['file', 'name', 'author', 'rating', 'description'])

    previews = []
    for row in data[['file', 'rating']].iloc[skip:take].itertuples():
        file_path = os.path.join(str(directory), 'previews', row.file)
        if (os.path.isfile(file_path)):
            fileObj = codecs.open(file_path, "r", "cp1251")
            text = fileObj.read()
            rating = row.rating
            previews.append((text, rating))
            fileObj.close()
    return previews

def write_model_params(take):
    file_path = os.path.join(base_dir, 'model_config.txt')
    with open(file_path, 'w') as file:
        text_to_write = str(take)
        file.write(text_to_write)

def read_model_params():
    file_path = os.path.join(base_dir, 'model_config.txt')
    if (not os.path.isfile(file_path)):
        return 0
    else:
        file_obj = codecs.open(file_path, "r", "utf_8_sig")
        text = file_obj.read()
        file_obj.close()
        return int(text)


# def get_all_ratings():
#     data = pd.read_csv(os.path.join(str(directory), 'description.csv'), delimiter=';',
#                        names=['file', 'name', 'author', 'rating', 'description'])
#
#     print(data['rating'].unique())
#
# get_all_ratings()