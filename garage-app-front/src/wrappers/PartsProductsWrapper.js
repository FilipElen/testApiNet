import React from "react";
import {Component} from "react";
import ProductsContainer from "../containers/ProductsContainer";
import MainCategories from "../utils/MainCategories";

class PartsProductsWrapper extends Component{
    render(){
        return(
            <ProductsContainer pageTitle="Parts" productCategory={MainCategories.PARTS}/>
        )
    }
}

export default PartsProductsWrapper;