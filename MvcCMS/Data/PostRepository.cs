using MvcCMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcCMS.Data {
    public class PostRepository : IPostRepository {
        public Post Get(string id) {
            using (var db = new CmsContext()) {
                return db.Posts.Include("Author")
                        .SingleOrDefault(x => x.Id == id);      
            }
        }

        public void Edit(string id, Post updatedItem) {
            using (var db = new CmsContext()) {
                var post = db.Posts.SingleOrDefault(x => x.Id == id);

                if (post == null) {
                    throw new KeyNotFoundException(string.Format("Нет post id : {0} в базе данных", id));
                }

                post.Id = updatedItem.Id;
                post.Title = updatedItem.Title;
                post.Content = updatedItem.Content;
                post.Published = updatedItem.Published;
                post.Tags = updatedItem.Tags;

                db.SaveChanges();
            }
        }

        public void Create(Post model) {
            using (var db = new CmsContext()) {
                var post = db.Posts.SingleOrDefault(x => x.Id == model.Id);

                if (post != null) {
                    throw new ArgumentException(string.Format("Этот post с id : {0} уже есть в базе данных", post.Id));
                }

                if (string.IsNullOrWhiteSpace(model.Id)) {
                    model.Id = model.Title;
                }

                model.Id = model.Id.MakeUrlFriendly();
                model.Tags = model.Tags.Select(x => x.MakeUrlFriendly()).ToList();

                db.Posts.Attach(model);
                db.SaveChanges();
            }
        }

        public IEnumerable<Post> GetAll() {
            using (var db = new CmsContext()) {
                return db.Posts.Include("Author")
                            .OrderByDescending(x => x.Create).ToArray();
            }
        }
    }
}