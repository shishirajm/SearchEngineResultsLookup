import styled, { css } from 'styled-components';

export const Container = styled.div``;

export const Header = styled.header`
  background-color: #909090;
  padding: 30px 40px;
  color: white;
  text-align: center;

  > div {
    display: flex;
    align-items: center;

    > input {
      outline: none;
      margin: 10px;
      border: none;
      border-radius: 0;
      width: 80%;
      padding: 10px;
      font-size: 16px;
      border-radius: 3px;
    }

    > select {
      font-size: 16px;
      font-weight: 700;
      color: #444;
      line-height: 1.3;
      padding: .6em 1.4em .5em .8em;
      width: 80%;
      max-width: 80%;
      box-sizing: border-box;
      margin: 0;
      border: 1px solid #aaa;
      border-radius: 0;
    }

    > a {
      display: block;
      align-items: center;
      text-decoration: none;
      border: 0;
      padding: 10px;
      width: 23%;
      background: #d9d9d9;
      color: #555;
      text-align: center;
      font-size: 16px;
      cursor: pointer;
      transition: 0.3s;
      border-radius: 0;
      margin-left: 2%;
      margin: 10px;
      border-radius: 3px;
    }

    > a:hover {
      background-color: #bbb;
    }
  }
`;
