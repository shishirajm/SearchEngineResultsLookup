import React from "react";
import { shallow, mount } from 'enzyme';
import Home from './index';


describe('Home', () => {
    const home = mount(<Home />);

    it('should render two input fields', () => {
        expect(home.find('input').length).toBe(2);
    });

    it('should render one select fields', () => {
        expect(home.find('select').length).toBe(1);
    });

    it('should render label fields upfront', () => {
        expect(home.find('h2').length).toBe(4);
    });

    it('should not render any result upfront', () => {
        expect(home.find('h3').length).toBe(0);
    });
});