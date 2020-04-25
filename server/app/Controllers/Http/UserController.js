'use strict'
/** @typedef {import('@adonisjs/framework/src/Request')} Request */
/** @typedef {import('@adonisjs/framework/src/Response')} Response */
/** @typedef {import('@adonisjs/framework/src/View')} View */
/**
 * Resourceful controller for interacting with users
 */

const Firebase = use('Perafan/Firebase');
const FirebaseAdmin = use('Perafan/FirebaseAdmin');

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

    Firebase.auth().signInWithEmailAndPassword(email, password)
      .catch(function (error) {
        console.log(error);
        // Handle Errors here.
        var errorCode = error.code;
        var errorMessage = error.message;
        if (errorCode === 'auth/wrong-password') {
          return { err: 'Wrong password.' }
        } else {
          return { err: errorMessage }
        }
        return { err: error }
      });
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

    const name = request.input('name')
    const email = request.input('email')
    const nickname = request.input('nickname')
    const birth_date = request.input('birth_date')
    const phone = request.input('phone')
    const password = request.input('password')
    const genre = request.input('genre')
    const private_profile = request.input('private_profile')

    const user = new User()
    user.name = name
    user.email = email
    user.nickname = nickname
    user.birth_date = birth_date
    user.phone = phone
    user.password = password
    user.genre = genre
    user.private_profile = private_profile

    await user.save()
    return response.json(user)
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
    let user = await User.query('id', params.id).fetch()
    return response.json(user)
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

Firebase.auth().onAuthStateChanged(function (user) {
  if (user) {
    console.log('Usuário logado!')
    return { user: user }
  }
  else {
    console.log('Login ou senha incorreta!')
    return { message: 'Login ou senha incorreta!' }
  }
});

module.exports = UserController
