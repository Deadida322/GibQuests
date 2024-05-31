import codecs
import pandas as pd
from dotenv import load_dotenv
import os

from preprocessing import preprocess_train_text, lemmed_and_clear_stop_words, clear_short_words, check_most_common_words

load_dotenv()
directory = os.getenv('DARA_DIR')
export_dir = 'process-data'
def get_texts(text_len = 3000):
    data = pd.read_csv(os.path.join(str(directory), 'description.csv'), delimiter=';',
                names=['file', 'name', 'author', 'rating', 'description'])

    previews = []
    for row in data[['file', 'rating']].head(text_len).itertuples():
        # print(row.file)
        file_path = os.path.join(str(directory), 'previews', row.file)
        if (os.path.isfile(file_path)):
            fileObj = codecs.open(file_path, "r", "cp1252" )
            text = fileObj.read()
            rating = row.rating
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
        file_path=os.path.join(str(directory), 'previews', row.file)
        export_file_path = os.path.join(export_dir, 'previews', row.file)
        if(not os.path.isfile(export_file_path)):
            file_obj = codecs.open(file_path, "r", "utf_8_sig" )
            text = file_obj.read()
            author = row.author

            text = preprocess_train_text(text, author)
            lemmed_words = lemmed_and_clear_stop_words(text)
            not_short_words = clear_short_words(lemmed_words, min_freq=0)

            with open(export_file_path, 'w') as file:
                text_to_write = ' '.join(not_short_words)
                print(text_to_write)
                file.write(text_to_write)
            file_obj.close()
