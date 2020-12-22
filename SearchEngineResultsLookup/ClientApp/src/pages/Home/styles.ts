import styled, { css } from 'styled-components';

export const Container = styled.div``;

export const Wrapper = styled.section`
  padding: 4em;
  background: #707070;
`

export const Title = styled.h1`
  font-size: 1.5em;
  text-align: center;
  color: white;
`

export const Label = styled.h2`
  font-size: 1.2em;
  text-align: left;
  color: white;
  width: 20%;
`

export const Result = styled.h3`
  font-size: 1.1em;
  text-align: left;
  color: white;
  width: 80%;
`

export const Section = styled.header`
  background-color: #909090;
  padding: 30px 40px;
  color: white;
  text-align: center;

  > div {
    display: flex;
    align-items: center;

    > a {
      padding: 0.5em;
      margin: 0.5em;
      display: block;
      align-items: center;
      text-decoration: none;
      border: 0;
      width: 90%;
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
`

export const Input = styled.input`
  padding: 0.5em;
  margin: 0.5em;
  color: black;
  background: White;
  border: none;
  border-radius: 3px;
  width: 70%;
  font-size: 16px;
  border-radius: 3px;
`

export const Select = styled.select`
  padding: 0.5em;
  margin: 0.5em;
  font-size: 16px;
  line-height: 1.3;
  width: 70%;
  max-width: 80%;
  box-sizing: border-box;
  margin: 0;
  border: 1px solid #aaa;
  border-radius: 0;
`;
