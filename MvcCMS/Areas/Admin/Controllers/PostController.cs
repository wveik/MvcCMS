﻿using MvcCMS.Data;
using MvcCMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcCMS.Areas.Admin.Controllers {
    // /admin/post
    [RouteArea("Admin")]
    [RoutePrefix("post")]
    public class PostController : Controller {

        private readonly IPostRepository _repository;

        public PostController()
            : this(new PostRepository()) { 
        
        }

        public PostController(IPostRepository repository) {
            _repository = repository;
        }

        // GET: Admin/Post
        public ActionResult Index() {
            var posts = _repository.GetAll();
            return View(posts);
        }

        // /admin/post/create/
        [HttpGet]
        [Route("create")]
        public ActionResult Create() {
            return View(new Post());
        }

        // /admin/post/create/ 
        [HttpPost]
        [Route("create")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Post model) {
            if (!ModelState.IsValid) {
                return View(model);
            }

            if (string.IsNullOrWhiteSpace(model.Id)) {
                model.Id = model.Title;
            }

            model.Id = model.Id.MakeUrlFriendly();
            model.Tags = model.Tags.Select(x => x.MakeUrlFriendly()).ToList();

            try {
                _repository.Create(model);

                return RedirectToAction("index");
            } catch (Exception ex) {
                ModelState.AddModelError("key", ex);
                return View(model);
            }
        }

        // /admin/post/edit/post-to-edit
        [HttpGet]
        [Route("edit/{postId}")]
        public ActionResult Edit(string postId) {
            //TODO: to retrieve the model from the data store

            var post = _repository.Get(postId);

            if (post == null)
                return HttpNotFound();

            return View(post);
        }

        // /admin/post/edit/post-to-edit
        [HttpPost]
        [Route("edit/{postId}")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string postId, Post model) {
            if (!ModelState.IsValid) {
                return View(model);
            }

            if (string.IsNullOrWhiteSpace(model.Id)) {
                model.Id = model.Title;
            }

            model.Id = model.Id.MakeUrlFriendly();
            model.Tags = model.Tags.Select(x => x.MakeUrlFriendly()).ToList();

            try {
                _repository.Edit(postId, model);

                return RedirectToAction("index");
            } catch (KeyNotFoundException ex) {
                return HttpNotFound();
            } catch (Exception ex) {
                ModelState.AddModelError("key", ex);
                return View(model);
            }
        }
    }
}