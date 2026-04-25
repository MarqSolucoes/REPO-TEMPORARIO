import axios from "axios";

export default {

    buscaCep: (cep, result) =>
    {
        axios.get(`https://viacep.com.br/ws/${ cep }/json/`)
				.then( (apiReturn) => result({ status: apiReturn.status, message: "", data: apiReturn.data }) )
				.catch( error => console.log(error) )
    }
}