import os
import sys
import datetime

import pandas as pd

import csv
import json

#import nest_asyncio

import warnings

from pandas.core.base import NoNewAttributesMixin

from DataExtractor import DataExtractor


def main():

    sites_file = r'C:\Users\Nadir\Documents\Parser_data\sites_test.txt'
    output_path = r'C:\Users\Nadir\Documents\Parser_data'
    timeout = 15

    with_rbk = False

    # Parse args 
    # TODO: create common func
    args_len = len(sys.argv)

    if args_len > 1:
        sites_file = sys.argv[1]

    if args_len > 2:
        output_path = sys.argv[2]

    if args_len > 3:
        try:
            timeout = int(sys.argv[3])
        except:
            pass

    if args_len > 4:
        with_rbk = sys.argv[4] == "true"

    with open(sites_file, 'r') as file:
        text = file.read().replace('\n', '')
    news_urls = json.loads(text)

    data_finder = DataExtractor(with_rbk, timeout)

    df = data_finder.parse(news_urls)

    if type(df) == type(None):
        return

    df = df.sort_values('Date', ascending=False)

    current_timestamp = datetime.datetime.now()
    current_timestamp = current_timestamp.strftime("%Y-%m-%d_%H-%M-%S")

    df.to_csv(os.path.join(output_path, f"{current_timestamp}.csv"), index=False)



if __name__ == "__main__":
    warnings.filterwarnings("ignore", 'This pattern has match groups')
    main()