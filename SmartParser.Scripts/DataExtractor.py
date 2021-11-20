from os import link
import pandas as pd

import asyncio
import aiohttp

import requests as req
import feedparser

from colorama import Fore, Style

from RBC_Parser import rbc_parser


class DataExtractor:

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


    async def _async_get(self, name, link):
        try:
            async with aiohttp.ClientSession() as session:
                async with session.get(link, timeout=self._timeout) as response:
                    res = await response.read()
                    if res != None:
                        print(rf'Loaded[{name}][{link}]', flush=True)
                    else:
                        print(rf'Broken[{name}][{link}]', flush=True)
                    return res
        except:
            print(rf'Broken[{name}][{link}]', flush=True)
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

        if self._with_rbk:
            links_length += 1

        print(f'!Loading[{links_length}]', flush=True)
        for name, url in urls.items():
            task = asyncio.ensure_future(self._async_get(name, url))
            tasks.append(task)


        get_results = await asyncio.gather(*tasks)
        errors_links = [link for i, link in enumerate(links) if get_results[i] == None]

        if self._with_rbk:
            task = asyncio.ensure_future(self._async_get(rbc_parser.get_name(), rbc_parser.get_link()))

            rbc_result = await task
            if rbc_result == None:
                errors_links.append(rbc_parser.get_link())

        print(f'!Completed[{links_length - len(errors_links)}]')


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

                print(f'Parsed[{names[i]}][{entries_count}]:{missed_cols}', flush=True)
                n_sites += 1
            except:
                print(f'Broken[{names[i]}][0]', flush=True)

        if self._with_rbk and rbc_result != None:

            try:
                single_df = self._single_parse(rbc_parser, rbc_parser.get_name(), rbc_result)
                dfs.append(single_df)

                missed_cols = single_df.columns[single_df[single_df['Name'] == rbc_parser.get_name()].isna().any()]
                missed_cols = missed_cols.tolist()
                entries_count = len(single_df)

                print(f'Parsed[RBC][{entries_count}]:{missed_cols}', flush=True)
                n_sites += 1
            except Exception as e:
                #print(str(e))
                print(f'Broken[RBC][0]', flush=True)

        
        len_df = 0
        df = None
        if len(dfs) != 0:
            df = pd.concat(dfs, ignore_index=True)
            len_df = len(df)

        print(f'!Completed[{n_sites}][{len_df}]', flush=True)

        return df


    def parse(self, urls):
        loop = asyncio.get_event_loop()
        return loop.run_until_complete(self._parse(urls))