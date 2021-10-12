from os import link
import pandas as pd

import asyncio
import aiohttp

import requests as req
import feedparser

from colorama import Fore, Style

from RBC_Parser import rbc_parser


class DataFinder:

    def __init__(self,
        with_rbk=False,
        timeout=10
    ) -> None:

        self._with_rbk = with_rbk
        self._timeout = timeout
        
        self._data_vocab = {
            'Name' : None,
            'Title' : 'title',
            'Description' : 'description',
            'Link' : 'link',
            'Date' : 'published'
        }


    async def _async_get(self, link):
        try:
            async with aiohttp.ClientSession() as session:
                async with session.get(link, timeout=self._timeout) as response:
                    res = await response.read()
                    if res != None:
                        print(r'#', flush=True)
                    return res
        except:
            return None


    def _single_parse(self, parser, name, source):

        df = pd.DataFrame(columns=self._data_vocab.keys())
        d = parser.parse(source)

        for entry in d['entries']:
            df_length = len(df)

            date = pd.to_datetime(entry.get(self._data_vocab['Date']))

            df.loc[df_length] = [
                name,
                entry.get(self._data_vocab['Title']),
                entry.get(self._data_vocab['Description']),
                entry.get(self._data_vocab['Link']),
                date.to_datetime64() if not date is None else None
            ]

        return df


    async def _parse(self, urls):

        all_entries_count = 0
        names = list(urls.keys())
        links = list(urls.values())
        links_length = len(links)

        dfs = []
        get_results = []
        tasks = []

        print(f'!Loading[{links_length}]', flush=True)
        for url in links:
            task = asyncio.ensure_future(self._async_get(url))
            tasks.append(task)

        get_results = await asyncio.gather(*tasks)
        errors_links = [link for i, link in enumerate(links) if get_results[i] == None]

        print(f'!Completed[{links_length - len(errors_links)}]:{errors_links}')


        if self._with_rbk:
            links_length += 1

        print(f'!Parsing[{links_length - len(errors_links)}]', flush=True)
        n_sites = 0
        for i, html in enumerate(get_results):
            if html == None:
                continue

            try:
                single_df = self._single_parse(feedparser, names[i], html)
                dfs.append(single_df)

                missed_cols = single_df.columns[single_df[single_df['Name'] == names[i]].isna().any()]
                missed_cols = missed_cols.tolist()
                entries_count = len(single_df)

                print(f'{names[i]}[{entries_count}]:{missed_cols}', flush=True)
                n_sites += 1
            except:
                print(f'{names[i]}[0]:error', flush=True)

        if self._with_rbk:

            try:
                single_df = self._single_parse(rbc_parser, 'RBC', '')
                dfs.append(single_df)

                missed_cols = single_df.columns[single_df[single_df['Name'] == names[i]].isna().any()]
                missed_cols = missed_cols.tolist()
                entries_count = len(single_df)

                print(f'RBC[{entries_count}]:{missed_cols}', flush=True)
                n_sites += 1
            except:
                print(f'RBC[0]:error', flush=True)


        df = pd.concat(dfs, ignore_index=True)
        print(f'!Completed[{n_sites}]:[{len(df)}]', flush=True)

        return df


    def parse(self, urls):
        loop = asyncio.get_event_loop()
        return loop.run_until_complete(self._parse(urls))