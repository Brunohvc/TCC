'use strict'
/** @typedef {import('@adonisjs/framework/src/Request')} Request */
/** @typedef {import('@adonisjs/framework/src/Response')} Response */
/** @typedef {import('@adonisjs/framework/src/View')} View */
/**
 * Resourceful controller for interacting with users
 */

const firebase = use('Perafan/Firebase');
const firebaseAdmin = use('Perafan/FirebaseAdmin');

class UserController {

  /**
  * Show a list of all users.
  * GET users
  *
  * @param {object} ctx
  * @param {Request} ctx.request
  * @param {Response} ctx.response
  * @param {View} ctx.view
  */
  async index({ response }) {
    let users = await User.query().fetch()
    return response.json(users)
  }

  async login({ request, response }) {
    const email = request.input('email')
    const password = request.input('password')
    console.log('Chegou na request', email, password);

    var res = null;
    await firebase.auth().signInWithEmailAndPassword(email, password)
      .then((user) => {
        if (!user.err) {
          console.log('signInWithEmailAndPassword: Usuário logado!')
          // var user = Firebase.auth().currentUser;
          res = response.json({ user: user, message: 'Success' })
        } else {
          console.log('signInWithEmailAndPassword: Login ou senha incorreta!')
          res = response.json({ message: 'Login ou senha incorreta!' })
        }
      })
      .catch(function (error) {
        // Handle Errors here.
        var errorCode = error.code;
        var errorMessage = error.message;

        console.log('signInWithEmailAndPassword: Login ou senha incorreta!')
        res = response.json({ message: errorMessage, code: errorCode })
      });

    console.log('res', res);
    return res;
  }


  /**
  * Create/save a new user.
  * POST users
  *
  * @param {object} ctx
  * @param {Request} ctx.request
  * @param {Response} ctx.response
  */
  async store({ request, response }) {
    const email = request.input('email')
    const password = request.input('password')
    console.log('Chegou na request', email, password);
    
    var res = null;
    await firebase.auth().createUserWithEmailAndPassword(email, password)
      .then((user) => {
        console.log('create', user)
        if (!user.err) {
          console.log('createUserWithEmailAndPassword: Usuário criado!')

          res = response.json({ user: user, message: 'Success' })
        } else {
          console.log('createUserWithEmailAndPassword: Login ou senha incorreta!')
          res = response.json({ message: 'Login ou senha incorreta!' })
        }
      })
      .catch(function (error) {
        // Handle Errors here.
        var errorCode = error.code;
        var errorMessage = error.message;

        console.log('createUserWithEmailAndPassword: Erro ao criar usuário!')
        res = response.json({ message: errorMessage, code: errorCode })
        // ...
      });

    return res
  }


  /**
  * Display a single user.
  * GET users/:id
  *
  * @param {object} ctx
  * @param {Request} ctx.request
  * @param {Response} ctx.response
  * @param {View} ctx.view
  */
  async show({ params, response }) {
    // let user = await User.query('id', params.id).fetch()
    // return response.json(user)
  }


  /**
  * Render a form to update an existing user.
  * GET users/:id/edit
  *
  * @param {object} ctx
  * @param {Request} ctx.request
  * @param {Response} ctx.response
  * @param {View} ctx.view
  */
  async edit({ params, request, response, view }) {
  }


  /**
  * Update user details.
  * PUT or PATCH users/:id
  *
  * @param {object} ctx
  * @param {Request} ctx.request
  * @param {Response} ctx.response
  */
  async update({ params, request, response }) {
    const name = request.input('name')
    const email = request.input('email')
    const nickname = request.input('nickname')
    const birth_date = request.input('birth_date')
    const phone = request.input('phone')
    const password = request.input('password')
    const genre = request.input('genre')
    const private_profile = request.input('private_profile')

    let user = await User.find(params.id)

    user.name = name
    user.email = email
    user.nickname = nickname
    user.phone = phone
    user.birth_date = birth_date
    user.password = password
    user.genre = genre
    user.private_profile = private_profile
    return response.json(user)
  }


  /**
  * Delete a user with id.
  * DELETE users/:id
  *
  * @param {object} ctx
  * @param {Request} ctx.request
  * @param {Response} ctx.response
  */
  async destroy({ params, request, response }) {

    let user = await User.find(params.id)

    user.status = false

    return response.json({ message: 'Contact deleted!' })
  }
}

// Firebase.auth().onAuthStateChanged(function (user) {
//   if (user) {
//     console.log('onAuthStateChanged: Usuário logado!')
//     return { user: user }
//   }
//   else {
//     console.log('onAuthStateChanged: Login ou senha incorreta!')
//     return { message: 'onAuthStateChanged Login ou senha incorreta!' }
//   }
// });

module.exports = UserController
