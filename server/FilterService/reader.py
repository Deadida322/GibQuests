import codecs
import pandas as pd
from dotenv import load_dotenv
import os

from preprocessing import preprocess_train_text, lemmed_and_clear_stop_words, clear_short_words, check_most_common_words

load_dotenv()
directory = os.getenv('DARA_DIR')
def get_texts(text_len = 3000):
    data = pd.read_csv(os.path.join(str(directory), 'description.csv'), delimiter=';',
                names=['file', 'name', 'author', 'rating', 'description'])

    previews = []
    for row in data[['file', 'rating']].head(text_len).itertuples():
        fileObj = codecs.open(os.path.join(str(directory), 'previews', row.file), "r", "utf_8_sig" )
        text = fileObj.read()
        rating = row.rating
        previews.append((text, rating))
        fileObj.close()
    print(len(previews))
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