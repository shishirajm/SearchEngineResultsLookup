import React, { ReactElement, useState, useEffect } from 'react';

import { Container, Header} from './styles';

export interface Lookup {
  date: string;
  temperatureC: number;
  temperatureF: number;
  summary: string;
}

export default function Home(): ReactElement {

  const initialState: number[] = [];
  const searchEngines: string[] = ['Google', 'Bing'];

  const [keyWord, setKeyWord] = useState('e-settlements');
  const [url, setUrl] = useState('www.sympli.com.au');
  const [searchEngine, setSearchEngine] = useState('Google');
  const [result, setResult] = useState(initialState);
  const [fetched, setFetched] = useState(false);
  const [error, setError] = useState({hasError: false, errorMsg: ''});
  const [loading, setLoading] = useState(false); 

  useEffect(() => {
    document.title = `${searchEngine} Lookup`;
  });

  function handleLookup(e: React.MouseEvent<HTMLAnchorElement, MouseEvent>): void {
    e.preventDefault();
    setLoading(true);
    setFetched(false);

    fetch(`api/lookup?keyWord=${keyWord}&url=${url}&searchEngine=${searchEngine}`)
      .then(response => response.json() as Promise<number[]>)    
      .then(data => {
        console.log(data);
        setResult(data);
        setError({hasError: false, errorMsg: ''});
        setFetched(true);
      })
      .catch(errorMsg => {
        console.log(errorMsg);
        setError({hasError: true, errorMsg: 'Error retrieving results. Check inputs.'});
      })
      .finally(() => {
        setLoading(false);
      });
  }

  return (
    <Container>
      <Header>
        <h2>Lookup Search Engine</h2>
        <div>
          Key Word:
          <input
              value={keyWord}
              onChange={(e): void => setKeyWord(e.target.value)}
              type="text"
              id="keyWord"
              placeholder="e-settlements"
            />
          </div>
          <div>
            Url to lookup:
            <input
              value={url}
              onChange={(e): void => setUrl(e.target.value)}
              type="text"
              id="url"
              placeholder="www.sympli.com.au"
            />
        </div>
        <div>
          Search Engine:
          <select name="searchEngine" value={searchEngine} onChange={(e) => {setSearchEngine(e.target.value)}}>
            {searchEngines.map(o => <option key={o} value={o}>{o}</option>)}
          </select>
        </div>
        <div><a href="/" onClick={(e): void => handleLookup(e)}>Lookup</a></div>
      </Header>
      {fetched &&
      <div>
        <p><b>{url}</b> found in <b>{result.length}</b> places.</p>
        <p>
          Position of Result: {result.join(', ')}
        </p>
      </div>
      } 
      { error.hasError &&
        <div>{error.errorMsg}</div>
      }
      {
        loading && <div>Finding the {url} in {keyWord} search on {searchEngine}.</div>
      }
    </Container>
  );
}
